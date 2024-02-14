using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 movedirection;
    public int movespeed;
    public Transform Mirror;

    public GameObject[] Hearts;
    public int life;
    public bool isRight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        move();
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
        if (Input.GetAxis("Horizontal") < 0 && isRight == true)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.GetChild(0).localScale = new Vector3(-transform.GetChild(0).transform.localScale.x, -transform.GetChild(0).transform.localScale.y, 1);
            isRight = false;
        }
        else if (Input.GetAxis("Horizontal") > 0 && isRight == false)
        {
            transform.localScale = new Vector3(1, 1, 1); 
            transform.GetChild(0).localScale = new Vector3(transform.GetChild(0).transform.localScale.x, transform.GetChild(0).transform.localScale.y, 1);
            isRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
        if (collision.transform.tag == "Enemy")
        {
            life -= 1;
        }
    }

    private void FixedUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            Mirror.gameObject.SetActive(true);
            Vector2 Mirdir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Mirror.position).normalized;
            Mirror.GetComponent<Rigidbody2D>().rotation = GetAngleFromVectorFloat(Mirdir);

        }
        else
        {
            Mirror.gameObject.SetActive(false);
        }

    }
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;

    }
    void move()
    {

        rb.velocity = new Vector2(movedirection.x * movespeed, movedirection.y * movespeed);
    }
    void Inputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movedirection = new Vector2(moveX, moveY).normalized;
    }
}
