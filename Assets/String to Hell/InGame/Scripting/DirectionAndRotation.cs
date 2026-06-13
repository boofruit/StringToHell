using StringToHell.InGame;
using UnityEngine;

namespace StringToHell.InGame
{
    public class DirectionAndRotation : MonoBehaviour, IDirectionAndRotation
    {
        Transform sr;
        Rigidbody2D rb;
        ISpiderInteractionContols SpiderPositon;

        void Awake() => rb = GetComponent<Rigidbody2D>();

        void Start()
        {
            SpiderPositon = GetComponent<ISpiderInteractionContols>();
            sr = GetComponentInChildren<SpriteRenderer>().transform;
        }
        bool IsReverse(Direction oldDirection, Direction newDirection)
        {
            return (oldDirection == Direction.Left && newDirection == Direction.Right) ||
                   (oldDirection == Direction.Right && newDirection == Direction.Left) ||
                   (oldDirection == Direction.Up && newDirection == Direction.Down) ||
                   (oldDirection == Direction.Down && newDirection == Direction.Up);
        }
        public void AirRotation()
        {
            // Rotate based on velocity direction, but only if there's significant movement
            Vector2 velocity = rb.linearVelocity;

            float rotationZ = 0f;
            if (velocity.magnitude > 1f)
            {
                if (currentInputDirection == Direction.Left)
                { rotationZ = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 180f; }
                else
                { rotationZ = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg; }
            }
            sr.rotation = Quaternion.Euler(0, 0, rotationZ); // Keep only Z rotation

        }
        public void RotateInstant(Vector2 normal)
        {
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
            // if ( !spiderCon.Clinging){ direction = rb.linearVelocity; }
            float rotationZ = Mathf.Atan2(SpiderPositon.SurfaceNormal.y, SpiderPositon.SurfaceNormal.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRot = Quaternion.Euler(0, 0, rotationZ); // Keep only Z rotation
            sr.rotation = Quaternion.Lerp(sr.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
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
                if (SpiderPositon.Clinging|| SpiderPositon.Grounded)
                { 
                sr.localScale = new Vector2((currentInputDirection == Direction.Left) ? -1 : 1, 1);

                }
            }
        }
    }
}