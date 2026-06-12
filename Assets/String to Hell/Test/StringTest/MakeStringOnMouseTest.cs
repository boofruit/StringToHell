using UnityEngine;
using StringToHell.InGame;

namespace StringToHell.Test.StringTest
{
    public class MakeStringOnMouseTest : MonoBehaviour
    {
        UnwindSilk unwind;
        Web webAnchor;
        SpriteRenderer sr;
        Transform tf;
        UnwindSilk lastWebAnchor;
        Vector2 placementPos;
        public bool ON_MOUSE = true;

        [SerializeField] float segmentSpacing = 0.25f;     // Distance between segments
        [SerializeField] float frequency = 8f;              // Elasticity strength
        [SerializeField] float dampingRatio = 0.6f;        // Reduces wobble
        [SerializeField] int maxSegementsLength = 20;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tf = transform;
            sr = GetComponent<SpriteRenderer>();
            webAnchor = GetComponent<Web>();
        }

        Vector2 PlacementPos()
        {
            if (ON_MOUSE)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return mousePos;
            }
            else
            {
                return tf.position;
            }
        }
        // Update is called once per frame
        void Update()
        {
           
            if (Input.GetMouseButtonDown(0))
            {
               
                var anchorObj = webAnchor.PlaceAnchor(PlacementPos());
                unwind = anchorObj.GetComponent<UnwindSilk>();
                unwind.StartThread(anchorObj.GetComponent<Rigidbody2D>(),this.gameObject,GetComponent<SpringJoint2D>());
                if (lastWebAnchor != null)
                {
                    lastWebAnchor.ConnectLine(anchorObj);
                }
            }
            if ( Input.GetMouseButton(0))
            {

               
               if (ON_MOUSE)
                {
                    tf.position = PlacementPos();
                }
                unwind.AddSegment(PlacementPos(), segmentSpacing, maxSegementsLength, frequency, dampingRatio);
                
            }

            if (Input.GetMouseButtonUp(0))
            {
                unwind.StopThread();
                lastWebAnchor = unwind;
            }
            if (Input.GetMouseButtonDown(1))
            {
                unwind.CutThread();
            }
        }
    }
}
