using UnityEngine;

public class Wind : MonoBehaviour
{
    AreaEffector2D wind;
    private float baseWindSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wind = GetComponent<AreaEffector2D>();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
        baseWindSpeed = wind.forceMagnitude;

        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
