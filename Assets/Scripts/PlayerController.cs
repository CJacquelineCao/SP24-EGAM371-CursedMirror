using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 movedirection;
    public int movespeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        move();
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
