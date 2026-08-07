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
        bool stringCooldown = false;
        private void Awake()
        {
            BaseSpring = GetComponent<SpringJoint2D>();
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
            

            if (spiderPosition.Clingable)
            {
                
                if (input.IsSpinnerOn && !stringCooldown)
                {
                    stringCooldown = true;
                    StartCoroutine(Wait.DoWait(0f, () =>
                    {
                        stringCooldown = false;
                    }));
                    spiderPosition.Clinging = true;
                    var anchorObj = web.PlaceAnchor(tf.position);
                    silk.ConnectLine(anchorObj, frequency, dampingRatio);
                    silk.StartThread(anchorObj.GetComponent<Rigidbody2D>(), segmentSpacing) ;
                    web.LastString = anchorObj;
                    
                }
            }
            else if (input.IsSpinnerOn && web.LastString != null)
            {
                silk.StartThread(web.LastString.GetComponent<Rigidbody2D>(), segmentSpacing);
            }

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
         
        }
    }
}
