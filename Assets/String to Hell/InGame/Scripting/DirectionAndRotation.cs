using StringToHell.InGame;
using UnityEngine;

namespace StringToHell.InGame
{
    public class DirectionAndRotation : MonoBehaviour, IDirectionAndRotation
    {
        Transform sr;
        Rigidbody2D rb;
        ISpiderInteractionContols SpiderPositon;
        IUnwindSilk silk;

        void Awake() => rb = GetComponent<Rigidbody2D>();

        void Start()
        {
            SpiderPositon = GetComponent<ISpiderInteractionContols>();
            sr = GetComponentInChildren<SpriteRenderer>().transform;
            silk = GetComponentInChildren<IUnwindSilk>();
        }
        bool IsReverse(Vector2 oldDirection, Vector2 newDirection)
        {
            return Vector2.Dot(oldDirection, newDirection) < -0.9f;
        }
        public void AirRotation()
        {
            // Rotate based on velocity direction, but only if there's significant movement
            Vector2 velocity = rb.linearVelocity;

            float rotationZ = 0f;
            if (velocity.magnitude > 1f)
            {
                bool leftFacing = false;
                if (currentInputDirection == Vector2.left) {leftFacing = true;}
                rotationZ = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + (leftFacing? 180f: 0f); 
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
                currentInputDirection = currentInputDirection == Vector2.left ? Vector2.right : Vector2.left;
                sr.localScale = new Vector2((currentInputDirection == Vector2.left) ? -1 : 1, 1);
            }
        }
        public void RotateBody(float rotationSpeed)
        {
            float rotationZ = Mathf.Atan2(SpiderPositon.SurfaceNormal.y, SpiderPositon.SurfaceNormal.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRot = Quaternion.Euler(0, 0, rotationZ); // Keep only Z rotation
            sr.rotation = Quaternion.Lerp(sr.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }

        Vector2 SurafeceDirection(Vector2 normal)
        {
            bool isVertical = Mathf.Abs(normal.x) > Mathf.Abs(normal.y);
            if (isVertical)
            {
                if (normal.x > 0)
                {
                    return Vector2.right;
                }
                else
                {
                    return Vector2.left;
                }
            }
            else
            {
                if (normal.y >= 0)
                {
                    return Vector2.up;
                }
                else
                {
                    return Vector2.down;
                }
            }
        }

        Vector2 currentInputDirection = Vector2.right;
        Vector2 InputDirection(Vector2 controllerInput)
        {
            float InputX = controllerInput.x;
            float InputY = controllerInput.y;
            Vector2 surfaceDirection = SurafeceDirection(SpiderPositon.SurfaceNormal);
            
            if(surfaceDirection == Vector2.up)
                return InputX > 0 ? Vector2.right : InputX < 0 ? Vector2.left : currentInputDirection;
            if(surfaceDirection == Vector2.down)
                return InputX > 0 ? Vector2.left : InputX < 0 ? Vector2.right : currentInputDirection;
            if(surfaceDirection != Vector2.left)
                return InputY > 0 ? Vector2.right : InputY < 0 ? Vector2.left : currentInputDirection;
            if(surfaceDirection == Vector2.right)
                return InputY > 0 ? Vector2.left : InputY < 0 ? Vector2.right : currentInputDirection;

            return currentInputDirection;
        }

        public void ChangeDirection(Vector2 newDirection)
        {     
            currentInputDirection = InputDirection(newDirection);
            if (newDirection != Vector2.zero)
            {
                Vector2 dir = InputDirection(-silk.SlingDirection);
                if (silk.Tugging && SpiderPositon.Clinging && currentInputDirection == dir)
                {      
                    Debug.Log("Tugging");
                    sr.localScale = new Vector2((currentInputDirection == Vector2.right) ? -1 : 1, 1);          
                }
                else
                {
                    // Debug.Log("normal input direction");
                    sr.localScale = new Vector2((currentInputDirection == Vector2.left) ? -1 : 1, 1);
                }       
            }
        }
    }
}