using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public float speed;          // Speed of the projectile
    public float lifetime;        // Lifetime of the projectile
    public GameObject hitEffect;        // Effect to play upon hitting a target

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set the velocity of the projectile
        rb.velocity = transform.right * speed;


        // Destroy the projectile after its lifetime
        Destroy(gameObject, lifetime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits an enemy
        if (other.CompareTag("Enemy"))
        {
            // Play hit effect
            //Instantiate(hitEffect, transform.position, Quaternion.identity);

            // Destroy the enemy
            other.gameObject.GetComponent<GenericEnemy>().SetState(GenericEnemy.AiState.Die);

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
