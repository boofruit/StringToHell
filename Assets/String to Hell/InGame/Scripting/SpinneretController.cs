using UnityEngine;

namespace StringToHell.InGame
{

    public class SpinneretController : MonoBehaviour
    {
        ISpiderInteractionContols spiderPosition;
        IUnwindSilk silk;
        IWeb web;
        IUnwindSilk lastWeb;
        IMovementInput input;
        IDirectionAndRotation RotationControls;
        IMovement movement;
        
        IVelocityController velocityController;
        
        [SerializeField] float segmentSpacing = 0.25f;     // Distance between segments
        [SerializeField] float frequency = 8f;              // Elasticity strength
        [SerializeField] float dampingRatio = 0.6f;        // Reduces wobble
        [SerializeField] int maxSegementsLength = 20;
        [SerializeField] float spacingMultiplier = 1.5f;
        SpringJoint2D BaseSpring;
        Transform tf;

        private void Awake()
        {
            BaseSpring = GetComponentInParent<SpringJoint2D>();
            BaseSpring.distance = segmentSpacing;
            BaseSpring.frequency = frequency;
            BaseSpring.dampingRatio = dampingRatio;
            silk = GetComponent<IUnwindSilk>();
            web = GetComponent<IWeb>();
            tf = GetComponent<Transform>();
            spiderPosition = GetComponentInParent<ISpiderInteractionContols>();
            RotationControls = GetComponentInParent<IDirectionAndRotation>();
            movement = GetComponentInParent<IMovement>();
            input = GetComponentInParent<IMovementInput>();
            velocityController = GetComponentInParent<IVelocityController>();
            //CreateParameter();
        }
        private void Update()
        {
            

            if (spiderPosition.Clinging || spiderPosition.Grounded)
            {
                
                if (input.IsSpinnerOn)
                {
                    var anchorObj = web.PlaceAnchor(tf.position);
                    silk.ConnectLine(anchorObj);
                    silk.StartThread(anchorObj.GetComponent<Rigidbody2D>(), segmentSpacing) ;
                   
                   
                    web.LastString = anchorObj;
                    
                }
            }
            //if (silk== null)
            //{
            //    return;
            //}
            
            if (input.IsSpinnerHold)
            {
                silk.AddSegment(maxSegementsLength, frequency, dampingRatio, spacingMultiplier);
            }
            if (input.IsSpinnerOff)
            {
                silk.StopThread();
            }
            if (input.IsCutWeb)
            {
                silk.CutThread();
            }
            if (spiderPosition.Clinging)
            {
                if (input.IsGrab || input.IsJump)
                {
                 // silk.BungieSling(slingForce);
                }
            }
        }
    }
}
