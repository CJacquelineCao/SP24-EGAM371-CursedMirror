using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float angleChangingSpeed;
    public float movementSpeed;

    public PlayerController playerref;

    public float elapsedTime;
    public float maxTime;
    public float t;

    public bool timerStarted;
    // Start is called before the first frame update
    void Start()
    {
        playerref = GameObject.FindObjectOfType<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        anotherone();
        if(timerStarted == true)
        {
            ReformTimer();
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void anotherone()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = targetPosition - transform.position;
        float angleToMouse = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angleToMouse));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * angleChangingSpeed);

        rigidBody.velocity = transform.up * movementSpeed;

    }

    public void ReformTimer()
    {
        elapsedTime += Time.deltaTime;

        t = maxTime - elapsedTime;

        if (t <= 0)
        {
            playerref.MirrorBroke = false;
            Destroy(gameObject);
        }
    }
    public void firedMod()
    {
        Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector3 deltaPosition = movementSpeed * dir * Time.deltaTime;
        rigidBody.MovePosition(transform.position + deltaPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Shard Enemy");
            collision.gameObject.GetComponent<GenericEnemy>().SetState(GenericEnemy.AiState.Die);
            playerref.MirrorBroke = false;
            Destroy(gameObject, 1f);
        }
        else
        {
            timerStarted = true;
        }
    }


}
