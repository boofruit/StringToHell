
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

        [SerializeField, Tooltip("")] float WallStop = .5f;
         [SerializeField, Tooltip("")] float WindStop = .5f;
        [SerializeField, Tooltip("")] float antiGravity = 0;
        [SerializeField, Tooltip("")] float gripStrength = 10;
        [SerializeField, Tooltip("")] float clingDampening = 10;
        [SerializeField, Tooltip("")] float snapStrength = 10;
        [SerializeField, Tooltip("")] float rotationSpeed = 2f;
        [SerializeField, Tooltip("")] float WallSwitchTimer = 1f;
        [SerializeField, Tooltip("")] float GroundCheckRadius = 0.5f;

        [SerializeField, Tooltip("")] string[] wallTags;

        Vector2 surfaceNormal = Vector2.up;
        public Vector2 SurfaceNormal => surfaceNormal;
        bool switchWalls = true;
        bool puff;
        public bool Puff => puff;
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
       public bool Clinging { get; set; }
       // public bool Clinging => clinging;
        bool grounded;
        public bool Grounded => grounded;
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
            if (grounded || Clinging == true)
            {
                Clinging = !Clinging;
                
            }
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
                    forceDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) ;
                }
            }
            if (tagC.CheckTags(wallTags, entering.tag))
            {
                Vector3 closest = collision.ClosestPoint(transform.position);

                // Compute the "normal" from that point to your object
                Vector3 normal = (transform.position - closest).normalized;

                if(WhenPlayerLeave(normal))
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
                Clinging = true;
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
                Debug.DrawRay(transform.position, -surfaceNormal * GroundCheckRadius, Color.red);
                if (Clinging)
                {
                   if (mC.Jumping) { return; }
                    rb.gravityScale = antiGravity;
                    if (!grounded )
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
                else {
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
                if ( !grounded)
                {
                   
                    Clinging = false;
                }
                rb.gravityScale = baseGravityMultiplier;
                currentWalls--;
            }

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var touching = collision.gameObject;

            if (tagC.CheckTags(wallTags, touching.tag))
            {
                grounded = true;
                //Clinging = true;
                jumpsLeft = MaxJumps;
              rb.linearVelocity *= WallStop;
                if(switchWalls)
                {
                    dR.RotateInstant(collision.GetContact(0).normal);
                    switchWalls = false;
                    StartCoroutine(WaitForSwitch());
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
                if (currentWalls <= 1)
                {
                }
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
            if (tagC.CheckTags(wallTags, touching.tag))
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
            bool isGrounded = Physics2D.CircleCast(transform.position, legsLength, -surfaceNormal,GroundCheckRadius, LayerMask.GetMask("Ground"));
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

