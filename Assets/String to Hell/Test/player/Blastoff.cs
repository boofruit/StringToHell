using UnityEngine;

public class Blastoff : MonoBehaviour
{
    [SerializeField] Space moveMode = Space.Self;
    [SerializeField] float speed = 50f;
    [SerializeField, Range( 0f, 1f )] float speedMultiplier = .5f;
    [SerializeField] KeyCode jump = KeyCode.Space;
    Rigidbody2D rb;
    public float jumpForce = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   //put instance summons here like keydown so they arent missed
    void Update()
    {
        if (Input.GetKeyDown(jump))
        {
            //.Impulse is meant for instant jumps in force, Start/On summon, GetKeyDown
            //.Force is meant for continuesly adding force, FixedUpdate, GetKey
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    //updates at the set frame rate, base 50fps, dont use time.deltatime
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        float y = Input.GetAxisRaw ("Vertical");
        float x = Input.GetAxisRaw("Horizontal");
        var move = new Vector3(x, y, 0);
        if (y > 0)
        {
            //rb.linearVelocityY = 0;
        }
        transform.Translate(move.normalized * speed * speedMultiplier, moveMode);
    }
}
