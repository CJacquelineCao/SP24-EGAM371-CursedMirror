using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : MonoBehaviour
{
    public BirdState currentState;
    public Transform player;
    public float rotationSpeed;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval;
    public float lastShootTime;
    public float maxRandomAngleChange = 30f;
    public float patrolRadius = 5f;
    public float patrolSpeed = 2f;
    public Transform patrolCenter;
    public GameObject Mirror;
    public bool isPatrolling;

    public float elapsedTime;
    public float maxTime;
    public float t;

    public bool CountDownStarted;

    public LineRenderer lineRenderer;

    public Sprite regularSprite;
    public Sprite angrySprite;

    public SpriteRenderer BirdRenderer;
    public enum BirdState
    {
        Guard,
        Snipe,
        Provoke,
        Die
    }
    public void SetState(BirdState newState)
    {
        currentState = newState;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Mirror = GameObject.FindGameObjectWithTag("Mirror");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case BirdState.Guard:
                GuardRoutine();
                break;

            case BirdState.Provoke:
                ProvokeRoutine();
                break;

            case BirdState.Snipe:
                SnipeRoutine();
                break;

            case BirdState.Die:
                //DeathRoutine();
                break;

        }
        if (CountDownStarted == true)
        {

            elapsedTime += Time.deltaTime;

            t = maxTime - elapsedTime;

        }
        else
        {
            t = maxTime;
            elapsedTime = 0;
        }
        if (t <= 0)
        {
            SetState(BirdState.Guard);
            CountDownStarted = false;
        }
    }

    void GuardRoutine()
    {
        if (!isPatrolling)
        {
            isPatrolling = true;
            BirdRenderer.sprite = regularSprite;
            StartCoroutine(Patrol());
        }

    }
    void ProvokeRoutine()
    {
        BirdRenderer.sprite = angrySprite;
        StartCoroutine(BumpIntoDecoy(Mirror.transform));
    }
    void SnipeRoutine()
    {
        RotateTowardsPlayer();
        DrawAimLine();
        if (Time.time - lastShootTime > shootInterval)
        {
            // Shoot towards the player
            Shoot();
            lastShootTime = Time.time;
        }
    }

    IEnumerator Patrol()
    {
        while (isPatrolling)
        {
            lineRenderer.enabled = false;
            float randomAngleChange = Random.Range(-maxRandomAngleChange, maxRandomAngleChange);
            float randomRadiusChange = Random.Range(-patrolSpeed, patrolSpeed) * Time.deltaTime;

            float x = Mathf.Cos(randomAngleChange) * (patrolRadius + randomRadiusChange);
            float y = Mathf.Sin(randomAngleChange) * (patrolRadius + randomRadiusChange);

            Vector3 targetPosition = patrolCenter.position + new Vector3(x, y, 0f);

            float elapsedTime = 0f;
            float duration = 1f; // Adjust the duration as needed

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final position is exactly the target position
            transform.position = targetPosition;

            yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
        }
    }
    void RotateTowardsPlayer()
    {
        Vector2 direction = player.position - firePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the firePoint to aim at the player
        Quaternion firePointRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        firePoint.rotation = firePointRotation;


    }
    void DrawAimLine()
    {
        // Set the starting position of the line to the fire point
        lineRenderer.SetPosition(0, firePoint.position);

        // Calculate the end position based on the fire point's rotation and a distance (adjust as needed)
        float lineLength = 10f;
        Vector3 endPosition = firePoint.position + Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z) * Vector3.right * lineLength;

        // Set the ending position of the line
        lineRenderer.SetPosition(1, endPosition);
    }
    void Shoot()
    {
        // Instantiate a bullet prefab and shoot it towards the player
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection((player.position - firePoint.position).normalized);
        }
    }
    IEnumerator BumpIntoDecoy(Transform currentDecoy)
    {
        // Bump into the decoy as long as the decoy exists
        while (currentDecoy.parent != player.gameObject.transform)
        {
            Vector2 direction = (currentDecoy.position - transform.position).normalized;
            float bumpForce = 10f;
            transform.position += (Vector3)direction * bumpForce * Time.deltaTime;

            yield return null;
        }
        SetState(BirdState.Guard);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Mirror")
        {
            if(player.GetComponent<PlayerController>().canThrow == false)
            {
                isPatrolling = false;
                lineRenderer.enabled = false;
                SetState(BirdState.Provoke);
            }

        }
        else if(collision.tag == "Player")
        {
            if(currentState != BirdState.Provoke)
            {
                isPatrolling = false;
                lineRenderer.enabled = true;
                SetState(BirdState.Snipe);
            }

        }
        else
        {
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CountDownStarted = true;
        }
    }
}
