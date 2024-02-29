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
    public Transform Mirror;

    public bool isMoving;
    public bool onGround;

    public Animator animationcontroller;
    public bool isRight;
    public GameObject[] Hearts;
    public int life;

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
        if (life < 1)
        {
            Hearts[0].gameObject.SetActive(false);


        }
        else if (life < 2)
        {
            Hearts[1].gameObject.SetActive(false);
        }
        else if (life < 3)
        {
            Hearts[2].gameObject.SetActive(false);

        }



    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Mirror.gameObject.SetActive(true);
            Vector2 Mirdir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Mirror.position).normalized;
            Debug.Log(Mirdir);
            Mirror.GetComponent<Rigidbody2D>().rotation = GetAngleFromVectorFloat1(Mirdir);
            //Mirror.GetComponent<Rigidbody2D>().MoveRotation(GetAngleFromVectorFloat1(Mirdir));

        }
        else
        {
            Mirror.gameObject.SetActive(false);
        }
    }
    public static float GetAngleFromVectorFloat1(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }

        return n;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
        if (collision.transform.tag == "Enemy")
        {
            life -= 1;
        }
        if (collision.transform.tag == "Ground")
        {
            onGround = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            onGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            onGround = false;
        }
    }


    void Walk(Vector2 dir)
    {
        Rb.velocity = new Vector2(dir.x * speed, Rb.velocity.y);

        isMoving = Mathf.Abs(Rb.velocity.x) > Mathf.Epsilon;

    }

}
