using NUnit.Framework.Internal;
using StringToHell.InGame;
using UnityEngine;
namespace StringToHell.InGame
{
    public class Movement : MonoBehaviour, IMovement
    {
        
        Transform tf;
        Transform sr;
        Rigidbody2D rb;
        [SerializeField] Space moveMode = Space.Self;
    
        bool horzontal;
        bool vertical;
        ISpiderInteractionContols SpiderPositon;
        private float Facing = 1.0f;

        float InputX;
        float InputY;

        void Awake() => rb = GetComponent<Rigidbody2D>();

        void Start()
        {
            SpiderPositon = GetComponent<ISpiderInteractionContols>();
            sr = GetComponentInChildren<SpriteRenderer>().transform;
            tf = transform;
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

        bool IsReverse(Direction oldDirection, Direction newDirection)
        {
            if ((oldDirection == Direction.Left && newDirection == Direction.Right) ||
                (oldDirection == Direction.Right && newDirection == Direction.Left) ||
                (oldDirection == Direction.Up && newDirection == Direction.Down) ||
                (oldDirection == Direction.Down && newDirection == Direction.Up))
            {
                return true;
            }
            return false;
        }
        public void RotateInstant(Vector2 normal)
        {
            //this.surfaceNormal = normal;
            Debug.Log("RotateInstant" + SpiderPositon.SurfaceNormal + " " + normal);
            float rotationZ = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg - 90f;
            sr.rotation = Quaternion.Euler(0, 0, rotationZ); // Keep only Z rotation

            var newDirection = SurafeceDirection(normal);
            var oldDirection = SurafeceDirection(SpiderPositon.SurfaceNormal);
            if (IsReverse(oldDirection, newDirection))
            {
                currentInputDirection = currentInputDirection == Direction.Left ? Direction.Right : Direction.Left;
                //newDirection = currentInputDirection;
                sr.localScale = new Vector2((currentInputDirection == Direction.Left) ? -1 : 1, 1);
            }
        }
        public void RotateBody(float rotationSpeed)
        {
            Debug.Log("RotateBody" + SpiderPositon.SurfaceNormal);
            // if ( !spiderCon.Clinging){ direction = rb.linearVelocity; }
            float rotationZ = Mathf.Atan2(SpiderPositon.SurfaceNormal.y, SpiderPositon.SurfaceNormal.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRot = Quaternion.Euler(0, 0, rotationZ); // Keep only Z rotation
            sr.rotation = Quaternion.Lerp(sr.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }

        public Vector2 JumpDirection(Vector2 controllerInput)
        {
            // If no input, jump straight out from the surface; otherwise, average normal and input
            if (controllerInput == Vector2.zero)
                return SpiderPositon.SurfaceNormal;
            else
                return (SpiderPositon.SurfaceNormal + controllerInput).normalized ;
        }

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        Direction SurafeceDirection(Vector2 normal)
        {
            bool isVertical = Mathf.Abs(normal.x) > Mathf.Abs(normal.y);
            if (isVertical)
            {
                if (normal.x > 0)
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Left;
                }
            }
            else
            {
                if (normal.y >= 0)
                {
                    return Direction.Up;
                }
                else
                {
                    return Direction.Down;
                }
            }
        }

        Direction currentInputDirection = Direction.Right;
        Direction InputDirection(Vector2 controllerInput)
        {
            float InputX = controllerInput.x;
            float InputY = controllerInput.y;
            Direction surfaceDirection = SurafeceDirection(SpiderPositon.SurfaceNormal);
            switch (surfaceDirection)
            {
                case Direction.Up:
                    return InputX > 0 ? Direction.Right : InputX < 0 ? Direction.Left : currentInputDirection;
                case Direction.Down:
                    return InputX > 0 ? Direction.Left : InputX < 0 ? Direction.Right : currentInputDirection;
                case Direction.Left:
                    return InputY > 0 ? Direction.Right : InputY < 0 ? Direction.Left : currentInputDirection;
                case Direction.Right:
                    return InputY > 0 ? Direction.Left : InputY < 0 ? Direction.Right : currentInputDirection;
                default:
                    return currentInputDirection;
            }
        }
        public void ChangeDirection(Vector2 newDirection)
        {
            currentInputDirection = InputDirection(newDirection);
            if (newDirection != Vector2.zero)
            {
                sr.localScale = new Vector2((currentInputDirection == Direction.Left) ? -1 : 1, 1);
            }
        }
    }
}