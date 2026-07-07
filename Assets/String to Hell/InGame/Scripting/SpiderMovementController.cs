using UnityEngine;

namespace StringToHell.InGame
{
    public class SpiderMovementController : MonoBehaviour
    {
        ISpiderInteractionContols spiderPosition;
        IUnwindSilk silk;
        IDirectionAndRotation RotationControls;
        IMovement movement;
        IMovementInput input;
        IVelocityController velocityController;
        //  [SerializeField, Tooltip("")]float rotationSpeedChangeRate = 10f;
        [SerializeField, Tooltip("")] float moveSpeed = 10f;
        [SerializeField, Tooltip("")] public float airSpeed = 5f;
        [SerializeField, Tooltip("")] public float maxSpeed = 6f;
        [SerializeField, Tooltip("")] float jumpPower = 1;
        [SerializeField, Tooltip("")] private float divePower = 1f;
        [SerializeField, Tooltip("")] private float floatPower = .5f;
        [SerializeField, Range(.0001f, 1f), Tooltip("")] float windResistanceMultiplier = .2f;

        [SerializeField, Tooltip("")] float pullStrength = 10;

        [SerializeField, Tooltip("")] float slingForce = 5f;
        [SerializeField, Tooltip("")] float minSlingTension = .5f;
        [SerializeField, Tooltip("")] float maxSlingForce = 100f;

        bool jumpQueued = false;
        bool slingjumpQueued = false;
        bool diveQueued = false;
        float moveSpeedChangeRate = 4f;

        MovementParameter movementParameter;

        private void Awake()
        {
            silk = GetComponentInChildren<IUnwindSilk>();
            spiderPosition = GetComponent<ISpiderInteractionContols>();
            RotationControls = GetComponent<IDirectionAndRotation>();
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
        private void FixedUpdate()
        {
            // this took a lot of trial and error to get right, but it works now. The jump is queued in the Update method, and then executed
            // in FixedUpdate to ensure that it happens at the right time in the physics cycle.   
            if (jumpQueued)
            {
                movement.Jump(movement.JumpDirection(input.Move), jumpPower);
                if (!spiderPosition.Clinging && silk.LineConnected)
                {
                    slingjumpQueued = true;
                }
                jumpQueued = false;
            }
            if (slingjumpQueued)
            {
                silk.BungieSling();
                slingjumpQueued = false;
            }
            if (diveQueued)
            {
                movement.Dive(input.Move, spiderPosition.ForceDirection, divePower, windResistanceMultiplier);
                movement.Float(input.Move, spiderPosition.ForceDirection, floatPower);
                diveQueued = false;
            }
        }
        private void Update()
        {


            if (!spiderPosition.Clinging)
            {
                if (input.IsDiving.magnitude > .9f)
                {
                    diveQueued = true;
                }

                if (!spiderPosition.Grounded)
                {
                    RotationControls.AirRotation();
                    movement.AirMovement(input.Move, airSpeed);
                }
            }
            if (spiderPosition.Grounded || spiderPosition.Clinging)
            {
                movement.WallMovement(input.Move, moveSpeed,
                    silk.LineConnected && spiderPosition.Clinging ||
                    spiderPosition.Puff && spiderPosition.Clinging ? pullStrength : 0);

                if (spiderPosition.Clinging && silk.LineConnected)
                {
                    silk.CalculateStrech(slingForce, minSlingTension, maxSlingForce);
                }

                if (input.IsJump)
                {
                    //circlecast
                    if (spiderPosition.CheckifGrounded())
                    {
                        jumpQueued = true;
                    }
                }


                if (input.IsGrab)
                {
                    spiderPosition.ClingSwitch();
                    if (!spiderPosition.Clinging && silk.LineConnected)
                    {
                        slingjumpQueued = true;
                    }
                }



                RotationControls.ChangeDirection(input.Move);

            }
        }
    }
}
