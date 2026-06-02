using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] KeyCode Jumpkey = KeyCode.Space;
    [SerializeField] KeyCode fallkey = KeyCode.S;
    [SerializeField] Space moveMode = Space.Self;
    [SerializeField] float speed;
    [SerializeField] float jumpPower = 1;
    [SerializeField] string[] targetTagsG;
    Transform tf;
    BoxCollider2D box;
   // GameObject walls;
    [SerializeField, Range (0f,100f)] private float timeScale = 1f;
    [SerializeField, Range (.0f,2f)] private float Yforce = 1f;
    bool plat = false;
    bool exit = false;
    bool grounded = false;
    int JumpsLeft = 1;

    

    void Start()
    {
        
       
        tf = transform;
       
        rb = GetComponent<Rigidbody2D>(); 
       // Time.timeScale = timeScale;
    }

   
    void Update()
    {
        
        if (plat && Input.GetKey(fallkey))
        {

            box.isTrigger = true; 
            grounded = false ;
            Debug.Log("fall");
        }
       
      
        
        
        
            jump();
        Move();
        // direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))*speed;
        //tf.Translate(direction);
    }
    void jump()
    {
        var time = Time.deltaTime * timeScale;
        if (grounded)
        {
            if (exit)
            {
                grounded = false;
            }
               
            if (Input.GetKeyDown(Jumpkey) && JumpsLeft > 0)//&& rb.linearVelocityY == 0;
            {
                rb.AddForceY((1) * jumpPower * Yforce, ForceMode2D.Impulse);
                JumpsLeft -= 1;
            }
        }
        
        if (!grounded)
        {
            if (Input.GetKey(Jumpkey))
            {

                var move = new Vector3(0, Yforce, 0);
                tf.Translate(move * time, moveMode);

            }
            if (Input.GetKey(fallkey))
            {
                rb.AddForce(new Vector2(0, -Yforce), ForceMode2D.Impulse);
                // rb.AddForceY( -1* (Time.deltaTime * timeScale));
            }
            if (!Input.GetKey(Jumpkey))
            {
                rb.AddForce(new Vector2(0, -Yforce / 3), ForceMode2D.Impulse);
            }
        }
        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

       var walls = collision.gameObject;
        
        if (walls.CompareTag("Platform" ))
            {
            box = walls.GetComponent<BoxCollider2D>();
            plat = true;
            Debug.Log("fall");
        }
        for (int i = 0; i < targetTagsG.Length; i++)
        {
            if (walls.CompareTag(targetTagsG[i]))
            {
                exit = false;
                grounded = true;
                JumpsLeft = 1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       var walls = collision.gameObject;
        
        if (walls.CompareTag("Platform"))
        {
            box = walls.GetComponent<BoxCollider2D>();
            plat = true;
            JumpsLeft += 1;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        var walls = collision.gameObject;
        if (walls.CompareTag("Platform"))
        {
            box.isTrigger = false;
            plat = false;
        }
        exit = true;
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var walls = collision.gameObject;
        if (walls.CompareTag("Platform"))
        {
            box.isTrigger = false;
            plat = false;
        }

    }
   
   void Move()
    {
        //float y = Input.GetAxisRaw ("Vertical");
        float x = Input.GetAxisRaw ("Horizontal");
        var move= new Vector3(x, 0, 0);
        tf.Translate(move * speed * Time.deltaTime, moveMode);
    }
}
