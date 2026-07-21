
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace StringToHell.InGame
{
    public class SpiderInteractionContols : MonoBehaviour, ISpiderInteractionContols
    {
        IDirectionAndRotation dR;
        IUnwindSilk silk;
        IMovement mC;
        TagCheck tagC;

        Rigidbody2D rb;

        [SerializeField, Range(-1, 1), Tooltip("the percentage rate linear velocity is reduced when the spider hits a wall")] float WallStop = .5f;
        [SerializeField, Range(-1, 1), Tooltip("the percentage rate linear velocity is reduced when the spider hits wind")] float WindStop = .5f;
        [SerializeField, Range(0, 1), Tooltip("the strength of the anti-gravity force while clinging")] float antiGravity = 0;
        [SerializeField, Range(0, 20), Tooltip("the strength of the grip force while clingint to ground directly")] float gripStrength = 10;
        [SerializeField, Range(0, 20), Tooltip("dampening when clinging to a surface while pulling thread or fighting wind")] float clingDampening = 10;
        [SerializeField, Range(0, 20), Tooltip("the strength of the pulling force towards ground while clinging")] float snapStrength = 10;
        [SerializeField, Range(0, 5), Tooltip("speed which spider sprite/collider rotates along a surface")] float rotationSpeed = 2f;
        [SerializeField, Range(0, 1), Tooltip("time to automatically rotate spider sprite/collider towards a new surface")] float WallSwitchTimer = 1f;
        [SerializeField, Range(0, 1), Tooltip("the radius of the ground check for jumping")] float GroundCheckRadius = 0.5f;

        [SerializeField, Tooltip("an array of tags that represent ground surfaces")] string[] wallTags;
        // represents the normal vector of the surface that the spider is currently in contact with.
        // It is used to determine the orientation of the spider relative to the surface and to apply forces accordingly.
        Vector2 surfaceNormal = Vector2.up;
        public Vector2 SurfaceNormal => surfaceNormal;
        bool switchWalls = true;
        // if the spider is in a wind zone, they are treated as a puffball
        bool puff;
        public bool Puff => puff;
        // represents the direction of the greatest force acting on the player, such as wind or gravity. It is used to determine the direction in which the playeris being pushed or pulled.
        Vector2 forceDirection = new Vector2(0, -1);
        public Vector2 ForceDirection => forceDirection;
        static int MaxJumps = 1;
        int jumpsLeft = 1;
        public int JumpsLeft => jumpsLeft;
        int currentWalls = 0;
        private float baseGravityMultiplier;
        private float baseDampening;
        float gravity;
        Transform tf;
        /// <summary>
        /// Indicates whether the spider can cling to a wall or surface. This is determined by the presence of a wall or surface within the spider's interaction range. 
        /// When true, the spider is able to cling to the surface; when false, it cannot.
        /// </summary>
        bool clingable;
        public bool Clingable => clingable;
        public bool AutoCling { get; set; } = true;
        // indicates if the spider is attempting to cling to a surface or string, and enables grabing behaviors when in a clingable state
        public bool Clinging { get; set; } = false;
        // public bool Clinging => clinging;
        bool grounded;
        public bool Grounded => grounded;
        bool isIce;
        public bool IsIce => isIce;
        //half the character height
        float legsLength;

        private void Awake()
        {
            legsLength = GetComponentInChildren<SpriteRenderer>().bounds.extents.y + 0.1f;
            tagC = GetComponent<TagCheck>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            jumpsLeft = MaxJumps;
            Debug.Log("Start?");
            rb = GetComponent<Rigidbody2D>();
            tf = transform;
            baseGravityMultiplier = rb.gravityScale;
            gravity = baseGravityMultiplier * 9.81f;
            baseDampening = rb.linearDamping;
            mC = GetComponent<IMovement>();
            dR = GetComponent<IDirectionAndRotation>();
            silk = GetComponentInChildren<IUnwindSilk>();
        }
        private void OnJointBreak2D(Joint2D joint)
        {
            //if( joint.enabled ) {return; }
            joint.enabled = false;
            joint.connectedBody = null;
            silk.Extinguish();
        }
        public void Jumpcalc(int Jmp)
        {
            Jmp = Mathf.Abs(Jmp);
            jumpsLeft -= Jmp;
        }

        public void ClingSwitch()
        {
            Clinging = !Clinging;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var entering = collision.gameObject;
            if (entering.CompareTag("Wind"))
            {
                puff = true;
                rb.linearVelocity *= WindStop;
                var wind = entering.GetComponent<AreaEffector2D>();

                if (wind.forceMagnitude > gravity)
                {
                    //Takes the force direction from the wind
                    float angle = wind.forceAngle;
                    float rad = angle * Mathf.Deg2Rad;
                    forceDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                }
            }
            if (tagC.CheckTags(wallTags, entering.tag))
            {
                if (AutoCling)
                {
                    Clinging = true;
                }
                Vector3 closest = collision.ClosestPoint(transform.position);

                // Compute the "normal" from that point to your object
                Vector3 normal = (transform.position - closest).normalized;

                if (WhenPlayerLeave(normal))
                {
                    Debug.Log("プレイヤーへの方向とプレイヤーの物理方向がだいたい一緒なので無視！");
                    return;
                }
                currentWalls++;
                surfaceNormal = normal;

                if (switchWalls || !Clinging)
                {
                    //dR.RotateInstant(surfaceNormal);
                    //switchWalls = false;
                    StartCoroutine(WaitForSwitch());
                    Debug.Log("cling");
                    // rb.AddForce(-surfaceNormal * snapStrength, ForceMode2D.Impulse); //problem child
                }
                clingable = true;
            }

        }

        private bool WhenPlayerLeave(Vector2 toPlayerDir)
        {
            bool sameDir = Vector2.Dot(toPlayerDir, rb.linearVelocity) > 0;
            return sameDir;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var entering = collision.gameObject;
            if (tagC.CheckTags(wallTags, entering.tag))
            {

                if (Clinging)
                {
                    if (mC.Jumping) { return; }
                    rb.gravityScale = antiGravity;
                    if (!grounded)
                    {
                        Vector3 closest = collision.ClosestPoint(transform.position);

                        // Compute the "normal" from that point to your object
                        Vector3 normal = (transform.position - closest).normalized;
                        surfaceNormal = normal;
                        rb.AddForce(-surfaceNormal * snapStrength, ForceMode2D.Force);
                    }
                    if (puff || silk.LineConnected)
                    {
                        rb.linearDamping = clingDampening;
                    }
                }
                else
                {
                    rb.gravityScale = baseGravityMultiplier;
                    rb.linearDamping = baseDampening;
                }

            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            var entering = collision.gameObject;
            if (entering.CompareTag("Wind"))
            {
                puff = false;
                forceDirection = new Vector2(0, -1);
            }
            if (tagC.CheckTags(wallTags, entering.tag))
            {

                clingable = false;

                rb.gravityScale = baseGravityMultiplier;
                currentWalls--;
            }
            isIce = false;

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var touching = collision.gameObject;

            if (touching.layer == LayerMask.NameToLayer("Ground"))
            {
                float dot = Vector2.Dot(collision.GetContact(0).normal, rb.linearVelocity);
                if (dot < 0)
                {
                    grounded = true;
                    jumpsLeft = MaxJumps;
                    rb.linearVelocity *= WallStop; // somtimes reduces velocity when trying to jump  thats why I added the dot check
                    if (switchWalls)
                    {
                        dR.RotateInstant(collision.GetContact(0).normal);
                        switchWalls = false;
                        StartCoroutine(WaitForSwitch());
                    }
                }
            }
          
        }
        IEnumerator WaitForSwitch()
        {
            yield return new WaitForSeconds(WallSwitchTimer);
            switchWalls = true;
        }
        //for every object your in contact with 
        private void OnCollisionStay2D(Collision2D collision)
        {
            Vector2 sum = Vector2.zero;

            foreach (var c in collision.contacts)
            { sum += c.normal; }

            surfaceNormal = (sum / collision.contactCount).normalized;

            //Debug.Log("Stay" + surfaceNormal);

            var touching = collision.gameObject;

            if (tagC.CheckTags(wallTags, touching.tag))
            {
                if (mC.Jumping) { return; }
                grounded = true;
                clingable = true;
                dR.RotateBody(rotationSpeed);
                jumpsLeft = MaxJumps;
                if (Clinging)
                {
                    rb.AddForce(-surfaceNormal * gripStrength, ForceMode2D.Force);
                }

            }
        }



        private void OnCollisionExit2D(Collision2D collision)
        {
            var touching = collision.gameObject;
            if (touching.layer == LayerMask.NameToLayer("Ground"))
            {

                grounded = false;
                rb.linearDamping = baseDampening;
                if (!switchWalls)
                {
                    StartCoroutine(WaitForSwitch());
                }
            }

        }
        public bool CheckifGrounded()
        {
            var touching = Physics2D.CircleCast(transform.position, legsLength, -surfaceNormal, GroundCheckRadius, LayerMask.GetMask("Ground"));
            bool isGrounded = touching;
            if (isGrounded)
            {
                isIce = touching.collider.CompareTag("Ice");
                Debug.Log("Grounded: " + isGrounded);
            }
            else { isIce = false; }
                grounded = isGrounded;
            //if (isGrounded)
            //{
            //    grounded = true;
            //    jumpsLeft = MaxJumps;
            //    Debug.Log("Grounded");
            //}
            //else
            //{
            //    grounded = false;
            //}
            return isGrounded;
        }

        private void OnDrawGizmos()
        {
            float radius = legsLength;
            float distance = GroundCheckRadius;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
            Vector3 endPos = (Vector2)transform.position + -surfaceNormal.normalized * distance;
            Gizmos.DrawWireSphere(endPos, radius);
            Gizmos.DrawLine(transform.position, endPos);
        }

    }
}

