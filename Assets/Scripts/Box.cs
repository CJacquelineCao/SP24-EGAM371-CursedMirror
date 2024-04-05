using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    // Start is called before the first frame update
    public float moveSpeed = 1f; // Speed at which the box moves
    public float gridSize = 1f; // Size of each grid space

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;
    public AudioSource pushSound;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!isMoving && IsPlayerTouching())
        {
            Move();
        }
    }

    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate movement direction based on input
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        // Check if the movement is along a single axis (not diagonal)
        if (Mathf.Abs(movement.x) > 0 && Mathf.Abs(movement.y) == 0)
        {
            // Move horizontally
            targetPosition += new Vector2(movement.x * gridSize, 0);
        }
        else if (Mathf.Abs(movement.y) > 0 && Mathf.Abs(movement.x) == 0)
        {
            // Move vertically
            targetPosition += new Vector2(0, movement.y * gridSize);
        }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(targetPosition, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Ground"))
            {
                // Move one space backward
                targetPosition -= movement * gridSize;
                break; // Exit the loop after adjusting the target position
            }
        }
        // Start moving towards the target position
        StartCoroutine(MoveTowardsTarget(targetPosition));
    }

    IEnumerator MoveTowardsTarget(Vector2 target)
    {
        isMoving = true;
        pushSound.Play();
        while (Vector2.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    private bool IsPlayerTouching()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

}



