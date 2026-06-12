using UnityEngine;
using UnityEngine.InputSystem;
namespace StringToHell.InGame
{
    public class PlayerInput : MonoBehaviour, IMovementInput
    {
        Vector2 move;
        public Vector2 Move => move;
        public float MoveMagnitude => move.magnitude;

        bool jump;
        Vector2 dive;
        bool spinnerOn;
        bool spinnerHold;
        bool spinnerOff;
        bool cutWeb;
        bool rewindString;
        bool grab;

        public bool IsJump => jump;
        public Vector2 IsDiving => dive;
        public bool IsSpinnerOn => spinnerOn;
        public bool IsSpinnerHold => spinnerHold;
        public bool IsSpinnerOff => spinnerOff;
        public bool IsCutWeb => cutWeb;
        public bool IsRewindString => rewindString; 
        public bool IsGrab => grab;
      



        [SerializeField] InputActionProperty moveActionProperty;
      
        [SerializeField] InputActionProperty webSpawnActionProperty;

        [SerializeField] InputActionProperty jumpActionProperty;

        [SerializeField] InputActionProperty diveActionProperty;

        [SerializeField] InputActionProperty cutWebActionProperty;

        [SerializeField] InputActionProperty rewindActionProperty;

        [SerializeField] InputActionProperty grabActionProperty;

        InputAction moveAction;
        InputAction jumpAction;
        InputAction webSpawnAction;
        InputAction cutWebAction;
        InputAction rewindStringAction;
        InputAction grabAction;
        InputAction diveAction;

        private void OnEnable()
        {
            grabAction = grabActionProperty.action;
            moveAction = moveActionProperty.action;
            jumpAction = jumpActionProperty.action;
            diveAction = diveActionProperty.action;
           
            cutWebAction = cutWebActionProperty.action;
            rewindStringAction = rewindActionProperty.action;
            webSpawnAction = webSpawnActionProperty.action;
        }

       
        private void Update()
        {
            jump = jumpAction.WasPressedThisFrame();
            move = moveAction.ReadValue<Vector2>();
            dive = diveAction.ReadValue<Vector2>();
            grab = grabAction.WasPressedThisFrame();

            spinnerOn = webSpawnAction.WasPressedThisFrame();
            spinnerHold = webSpawnAction.IsPressed();
            spinnerOff = webSpawnAction.WasReleasedThisFrame();
            cutWeb = cutWebAction.IsPressed();
            rewindString = rewindStringAction.IsPressed();
   
        }

    }

}
