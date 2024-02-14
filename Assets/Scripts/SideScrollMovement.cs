using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollMovement : MonoBehaviour
{
    public Vector3 velocity;
    public int speed;
    public Sprite regular;
    public int jumpForce;
    private Rigidbody2D Rb;

    public bool isMoving;
    public bool onGround;

    public float glidefallSpeed;

    public Animator animationcontroller;
    public bool isRight;


    // Start is called before the first frame update
    void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround == true)
            {
                Rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            }

        }

        if (onGround == false)
        {
            //animationcontroller.SetBool("Walking", false);

            if (Input.GetKey(KeyCode.Space))
            {
                Vector2 v = Rb.velocity;
            }

            

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {

                //animationcontroller.SetBool("Walking", true);

            }
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {

                //animationcontroller.SetBool("Walking", false);

            }
         
        }

        if (Input.GetAxis("Horizontal") < 0 && isRight == true)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            isRight = false;
        }
        else if (Input.GetAxis("Horizontal") > 0 && isRight == false)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            isRight = true;
        }

    }
    void Walk(Vector2 dir)
    {
        Rb.velocity = new Vector2(dir.x * speed, Rb.velocity.y);

        isMoving = Mathf.Abs(Rb.velocity.x) > Mathf.Epsilon;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            onGround = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            onGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            onGround = false;
        }
    }
}
