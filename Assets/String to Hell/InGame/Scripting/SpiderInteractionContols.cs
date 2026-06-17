
using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField, Tooltip("")] float pullStrength = 10;
        [SerializeField, Tooltip("")] float snapStrength = 10;
        [SerializeField, Tooltip("")] float rotationSpeed = 2f;
        [SerializeField, Tooltip("")] float WallSwitchTimer = 1f;

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
        
        private void Awake()
        {
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
                    float angle = wind.forceAngle;
                    float rad = angle * Mathf.Deg2Rad;
                    forceDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) ;
                }
            }
            if (tagC.CheckTags(wallTags, entering.tag))
            {
                currentWalls++;
                Vector3 closest = collision.ClosestPoint(transform.position);

                // Compute the "normal" from that point to your object
                Vector3 normal = (transform.position - closest).normalized;
                surfaceNormal = normal;
                if (switchWalls || !Clinging)
                {
                    dR.RotateInstant(surfaceNormal);
                    switchWalls = false;
                    StartCoroutine(WaitForSwitch());
                    rb.AddForce(-surfaceNormal * snapStrength, ForceMode2D.Impulse);
                }
                Clinging = true;
            }
           
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            var entering = collision.gameObject;
            if (tagC.CheckTags(wallTags, entering.tag))
            {
                if (Clinging)
                {
                    rb.gravityScale = antiGravity;
                    if (!grounded)
                    {
                        rb.AddForce(-surfaceNormal * snapStrength, ForceMode2D.Force);
                    }
                }
                else { rb.gravityScale = baseGravityMultiplier; }
                
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
                if (currentWalls <= 1 && !grounded)
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
                Clinging = true;
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
                grounded = true;
                if (currentWalls <= 1)
                {
                }
                dR.RotateBody(rotationSpeed);
                jumpsLeft = MaxJumps;
                if (Clinging)
                {
                    rb.AddForce(-surfaceNormal * gripStrength, ForceMode2D.Force);
                    if (puff || silk.LineConnected)
                    {
                        rb.linearDamping = pullStrength;
                    }
                   
                }
                else { rb.linearDamping = baseDampening; }
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

    }

}

