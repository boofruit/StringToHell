using UnityEngine;


namespace StringToHell.Test.StringTest
{
    public class MakeStringOnMouseTest : MonoBehaviour
    {
        StringUnwind unwind;
        WebAnchor webAnchor;
        SpriteRenderer sr;
        Transform tf;
        StringUnwind lastWebAnchor;
        Vector2 placementPos;
        public bool ON_MOUSE = true;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tf = transform;
            sr = GetComponent<SpriteRenderer>();
            webAnchor = GetComponent<WebAnchor>();
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
                unwind = anchorObj.GetComponent<StringUnwind>();
                unwind.StartThread(anchorObj.GetComponent<Rigidbody2D>(),this.gameObject);
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
                unwind.AddSegment(PlacementPos());
                
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
