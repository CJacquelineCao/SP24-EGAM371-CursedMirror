using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
    public GameObject Player;
    public AiState currentState;
    public int AttackPower;

    public int patrolPointIndex = 0;
    public PatrolPointsData patrolPointsData;

    public float remainingDistance;
    public float movespeed;

    public Rigidbody2D rb;
    public LayerMask playerMask;
    public float maxDistance;

    public float elapsedTime;
    public float maxTime;
    public float t;

    public bool CountDownStarted;

    public SpriteRenderer enemySprite;
    public Sprite PatrolSprite;
    public Sprite ChaseSprite;
    public Sprite DeadSprite;
    public enum AiState
    {
        Patrol,
        Chase,
        Special,
        Die
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    public void SetState(AiState newState)
    {
        //navMeshAgent.isStopped = true;
        currentState = newState;
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        switch (currentState)
        {
            case AiState.Patrol:
                PatrolRoutine();
                break;

            case AiState.Chase:
                ChaseRoutine();
                break;

            case AiState.Die:
                DieRoutine();
                break;
            case AiState.Special:
                break;

        }
        lineofSightCheck();
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
            SetState(AiState.Patrol);
            CountDownStarted = false;
        }
        if(remainingDistance >=30)
        {
            SetState(AiState.Die);
        }
    }
    public void lineofSightCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, maxDistance, playerMask);
        Debug.DrawRay(transform.position, -transform.right * maxDistance, Color.green);
        if (hit)
        {
            if(hit.transform.tag == "Player")
            {
                CountDownStarted = false;
                Debug.Log("i see u");
                SetState(AiState.Chase);
                if (Player.GetComponent<PlayerController>().MirrorOut == true)
                {
                    gameObject.GetComponent<GenericEnemy>().SetState(GenericEnemy.AiState.Special);
                }
            }
            else
            {

            }



        }
        else
        {
            CountDownStarted = true;

        }
    }

    public void PatrolRoutine()
    {
        enemySprite.GetComponent<SpriteRenderer>().sprite = PatrolSprite;
        Vector3 pos = Vector3.MoveTowards(transform.position, patrolPointsData.points[patrolPointIndex], movespeed * Time.deltaTime);
        rb.MovePosition(pos);
        remainingDistance = Vector3.Distance(gameObject.transform.position, patrolPointsData.points[patrolPointIndex]);
        transform.right = (patrolPointsData.points[patrolPointIndex] - transform.position).normalized *-1;
        if (remainingDistance < 0.5f)
        {
            patrolPointIndex++;
            if (patrolPointIndex >= patrolPointsData.points.Length)
            {
                patrolPointIndex = 0;
            }

        }
    }

    public void ChaseRoutine()
    {
        enemySprite.GetComponent<SpriteRenderer>().sprite = ChaseSprite;
        Vector3 pos = Vector3.MoveTowards(transform.position, Player.transform.position, movespeed * Time.deltaTime);
        //look at
        rb.MovePosition(pos);
        transform.right = (Player.transform.position - transform.position).normalized *-1;
    }

    public void DieRoutine()
    {
        enemySprite.GetComponent<SpriteRenderer>().sprite = DeadSprite;
        Destroy(gameObject, 1f);
    }
}
