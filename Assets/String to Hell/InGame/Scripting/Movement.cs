using NUnit.Framework.Internal;
using StringToHell.InGame;
using UnityEngine;
namespace StringToHell.InGame
{
    public class Movement : MonoBehaviour, IMovement
    {
        Transform tf;
        Rigidbody2D rb;
        [SerializeField] Space moveMode = Space.Self;

        ISpiderInteractionContols SpiderPositon;
        bool canDive = true;
        bool floating = false;
        void Awake() => rb = GetComponent<Rigidbody2D>();

        void Start()
        {
            SpiderPositon = GetComponent<ISpiderInteractionContols>();
            tf = transform;
        }
        void Update()
        {
            if (SpiderPositon.Clinging)
            {
                canDive = true;
                floating = false;
            }
        }
        public void WallMovement(Vector2 controllerInput, float moveSpeed)
        {
            // Project input onto the surface plane
            Vector2 move = controllerInput - Vector2.Dot(controllerInput, SpiderPositon.SurfaceNormal) * SpiderPositon.SurfaceNormal;
            tf.Translate(move * moveSpeed * Time.deltaTime, moveMode);
        }

        public void AirMovement(Vector2 controllerInput, float airSpeed)
        {
            // Apply force only from input
            tf.Translate(controllerInput * airSpeed * Time.deltaTime, moveMode);
        }

        //public void AirDrag(float windResistanceMultiplier)
        //{
        //    if (SpiderPositon.Puff)
        //    {
        //        rb.velocity *= (1 - windResistanceMultiplier);
        //    }
        //}
        public void Float(Vector2 diveDirection, float divePower)
        {
            
            if (SpiderPositon.Puff && !floating)
            {
               //rb.linearVelocity *= 0f;
                rb.AddForce(-diveDirection * (divePower/4f), ForceMode2D.Impulse);
                canDive = true;
                floating = true;
            }
        }

        public void Dive(Vector2 diveDirection, Vector2 inputDirection, float divePower, float windMultiplier)
        {
            if (canDive && IsWithinAngle(diveDirection,inputDirection, 45f))
            {
                if (!SpiderPositon.Puff)
                {
                rb.linearVelocity *= 0f;
                }
                rb.AddForce(diveDirection * divePower * (SpiderPositon.Puff ? windMultiplier : 1), ForceMode2D.Impulse);
                canDive = false;
                floating = false;
            }
        }
        bool IsWithinAngle(Vector2 v, Vector2 dir, float maxAngleDegrees)
        {
            // Ensure direction is normalized
            dir = dir.normalized;

            // Normalize v (unless it's zero)
            Vector2 vNorm = v.normalized;

            // Compute dot product
            float dot = Vector2.Dot(vNorm, dir);

            // Compare with cosine of allowed angle
            float cosThreshold = Mathf.Cos(maxAngleDegrees * Mathf.Deg2Rad);

            return dot >= cosThreshold;
        }

        public void Jump(Vector2 direction, float jumpPower)
        {
            if ( SpiderPositon.JumpsLeft > 0)
            {
                rb.AddForce(direction * jumpPower, ForceMode2D.Impulse);
                SpiderPositon.Jumpcalc(-1);
            }
        }

        public Vector2 JumpDirection(Vector2 controllerInput)
        {
            // If no input, jump straight out from the surface; otherwise, average normal and input
            if (controllerInput == Vector2.zero)
                return SpiderPositon.SurfaceNormal;
            else
                return (SpiderPositon.SurfaceNormal + controllerInput).normalized ;
        }       
    }
}