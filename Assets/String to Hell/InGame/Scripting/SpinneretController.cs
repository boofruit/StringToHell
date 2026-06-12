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

        Transform tf;

        private void Awake()
        {
            web = GetComponent<IWeb>();
            tf = GetComponentInParent<Transform>();
            spiderPosition = GetComponentInParent<ISpiderInteractionContols>();
            RotationControls = GetComponentInParent<IDirectionAndRotation>();
            movement = GetComponentInParent<IMovement>();
            input = GetComponentInParent<IMovementInput>();
            velocityController = GetComponentInParent<IVelocityController>();
            //CreateParameter();
        }
        private void Update()
        {
            

            if (spiderPosition.Clinging)
            {
                if (input.IsSpinnerOn)
                {
                    var anchorObj = web.PlaceAnchor(tf.position);
                    silk = anchorObj.GetComponent<IUnwindSilk>();
                    silk.StartThread(anchorObj.GetComponent<Rigidbody2D>(), this.gameObject, GetComponentInParent<SpringJoint2D>());
                    if (lastWeb != null)
                    {
                        lastWeb.ConnectLine(anchorObj);
                    }
                }
            }
            if (silk== null)
            {
                return;
            }
            silk.UpdateLineRenderer(segmentSpacing);
            if (input.IsSpinnerHold)
            {
                silk.AddSegment(tf.position, segmentSpacing, maxSegementsLength, frequency, dampingRatio);
            }
            if (input.IsSpinnerOff)
            {
                silk.StopThread();
                lastWeb = silk;
            }
            if (input.IsCutWeb)
            {
                silk.CutThread();
            }
            if (!spiderPosition.Clinging)
            {

            }
        }
    }
}
