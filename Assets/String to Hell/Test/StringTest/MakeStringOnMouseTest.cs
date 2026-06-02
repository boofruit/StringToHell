using UnityEngine;


namespace StringToHell.Test.StringTest
{
    public class MakeStringOnMouseTest : MonoBehaviour
    {
       public StringUnwind unwind;
        WebAnchor anchor;
        SpriteRenderer sr;
        Transform tf;
        void OnMouseDown()
        {
           
        }
        void OnMouseDrag()
        {
         
        }
        void OnMouseUp()
        {
           
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tf = transform;
            sr = GetComponent<SpriteRenderer>();
            anchor = GetComponent<WebAnchor>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var anchorObj = anchor.PlaceAnchor(mousePos);
                
                unwind.StartThread(anchorObj.GetComponent<Rigidbody2D>(), this.gameObject);
            }
            if ( Input.GetMouseButton(0))
            {

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tf.position = mousePos;
                unwind.AddSegment(mousePos);
            }

            if (Input.GetMouseButtonUp(0))
            {
                unwind.StopThread();
            }
            
        }
    }
}
