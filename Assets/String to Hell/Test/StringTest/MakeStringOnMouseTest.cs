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
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tf = transform;
            sr = GetComponent<SpriteRenderer>();
            webAnchor = GetComponent<WebAnchor>();
        }
      
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var anchorObj = webAnchor.PlaceAnchor(mousePos);
                unwind = anchorObj.GetComponent<StringUnwind>();
                unwind.StartThread(anchorObj.GetComponent<Rigidbody2D>(),this.gameObject);
                lastWebAnchor.ConnectLine(anchorObj.gameObject);

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
                lastWebAnchor = unwind;
            }
            if (Input.GetMouseButtonDown(1))
            {
                unwind.CutThread();
            }
        }
    }
}
