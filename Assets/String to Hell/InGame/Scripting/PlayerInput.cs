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
        bool triggerOn;
        bool triggerHold;
        bool triggerOff;

        public bool IsJump => jump;
        public Vector2 IsDiving => dive;
        public bool IsTriggerOn => triggerOn;
        public bool IsTriggerHold => triggerHold;
        public bool IsTriggerOff => triggerOff;
      



        [SerializeField]
        InputActionProperty moveActionProperty;
      
        [SerializeField] InputActionProperty triggerActionProperty;

        [SerializeField] InputActionProperty jumpActionProperty;

        [SerializeField] InputActionProperty diveActionProperty;

        InputAction moveAction;
        InputAction jumpAction;
        InputAction triggerAction;
        InputAction diveAction;

        private void OnEnable()
        {
            
            moveAction = moveActionProperty.action;
            jumpAction = jumpActionProperty.action;
            diveAction = diveActionProperty.action;
           
            triggerAction = triggerActionProperty.action;
        }

       
        private void Update()
        {
            jump = jumpAction.WasPressedThisFrame();
            move = moveAction.ReadValue<Vector2>();
            dive = diveAction.ReadValue<Vector2>();


            triggerOn = triggerAction.WasPressedThisFrame();
            triggerHold = triggerAction.IsPressed();
            triggerOff = triggerAction.WasReleasedThisFrame();
   
        }

    }

}
