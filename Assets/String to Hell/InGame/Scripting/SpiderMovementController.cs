using UnityEngine;

namespace StringToHell.InGame
{
    public class SpiderMovementController : MonoBehaviour
    {
        ISpiderInteractionContols SpiderIC;
        IMovement movement;
        IMovementInput input;
        IVelocityController velocityController;
      //  [SerializeField, Tooltip("")]float rotationSpeedChangeRate = 10f;
        [SerializeField, Tooltip("")] float moveSpeed = 10f;
        [SerializeField, Tooltip("")]public float airSpeed = 5f;
        [SerializeField, Tooltip("")] public float maxSpeed = 6f;
        [SerializeField, Tooltip("")] float jumpPower = 1;
        [SerializeField, Tooltip("")] private float DivePower = 1f;
        [SerializeField, Range(.0001f, 1f), Tooltip("")] float windResistanceMultiplier = .2f;

        float moveSpeedChangeRate = 4f;

        MovementParameter movementParameter;
      
        private void Awake()
        {
            SpiderIC = GetComponent<ISpiderInteractionContols>();
            movement = GetComponent<IMovement>();
            input = GetComponent<IMovementInput>();
            velocityController = GetComponent<IVelocityController>();
            //CreateParameter();
        }
        // onValidate is a Unity-specific method that is called when the script is loaded or a value is changed in the Inspector,
        // allowing for real-time updates and validation of the movement and rotation parameters during development
        //private void OnValidate()
        //{
        //    CreateParameter();
        //}
        //void CreateParameter()
        //{
        //    movementParameter = new MovementParameter(moveSpeed);
        //}
           
        private void Update()
        {
            if (input.IsJump)
            {
                SpiderIC.Jump(movement.JumpDirection(input.Move), jumpPower);
            }
            if (input.IsDiving.y < 0 && !SpiderIC.Clinging)
            {
                SpiderIC.Dive(DivePower, windResistanceMultiplier);
            }
            if (!SpiderIC.Clinging)
            {
                movement.AirMovement(input.Move, airSpeed);
            }
            if(SpiderIC.Clinging)
            {
                movement.WallMovement(input.Move, moveSpeed);
            
            }
            movement.ChangeDirection(input.Move);
        }
    }
}
