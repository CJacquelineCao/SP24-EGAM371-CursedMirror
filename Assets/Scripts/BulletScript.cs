using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Vector2 direction;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set the initial velocity based on the direction and speed
        rb.velocity = direction * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet collided with a wall (tag it as "Wall" or adjust as needed)
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the bullet upon collision with a wall
        }
        else if(collision.gameObject.CompareTag("Respawn"))
        {
            Destroy(gameObject);
        }
    }
    // Set the direction for the bullet to move
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }



}
