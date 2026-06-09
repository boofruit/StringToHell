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

        public void Dive(Vector2 diveDirection, float divePower, float windMultiplier)
        {
            if (canDive)
            {
                rb.AddForce(diveDirection * divePower * (SpiderPositon.Puff ? windMultiplier : 1), ForceMode2D.Impulse);
                canDive = false;
            }
        }
        public void Jump(Vector2 direction, float jumpPower)
        {
            if (SpiderPositon.Clinging && SpiderPositon.JumpsLeft > 0)
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