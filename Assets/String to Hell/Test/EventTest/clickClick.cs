using UnityEngine;

public class clickClick : MonoBehaviour
{
    SpriteRenderer sr;
    Transform tf;
    private Camera MainCamera;
    Vector3 offset;
    public Sprite changeSprite;
    Sprite origin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origin = GetComponent<SpriteRenderer>().sprite;
        MainCamera = Camera.main;
        tf = transform;
       sr = GetComponent<SpriteRenderer>(); 
        
    }
    private void OnMouseDrag()
    {
        float f = 3.14f;
        int a = (int)f;
        Debug.Log("a:"+ a);
        tf.position = GetClickPos() + offset;
        sr.color = Color.red;
    }
    private void OnMouseEnter()
    {
        sr.color = Color.yellow;
        sr.sprite = changeSprite;
    }
    private void OnMouseExit()
    {
        sr.color = Color.white;
        sr.sprite = origin;
        //Vector3.Scale = ;
    }
    private void OnMouseDown()
    {
        
        offset =  tf.position - GetClickPos();
    }
    void OnMouseUp()
    {
        sr.color = Color.white;
    }
    Vector3 GetClickPos()
    {
        var clickPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0;
        return clickPos;
    }
}
