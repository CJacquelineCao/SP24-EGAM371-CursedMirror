using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Horse : MonoBehaviour
{
    public GameObject Player;
    public AiState currentState;
    public List<Vector3> patrolPointList = new List<Vector3>();
    public int patrolPointIndex = 0;

    public float remainingDistance;
    public float movespeed;

    public Rigidbody2D rb;
    public LayerMask playerMask;
    public float maxDistance;

    public float elapsedTime;
    public float maxTime;
    public float t;

    public bool CountDownStarted;
    public enum AiState
    {
        Patrol,
        Chase,
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
    }
    public void lineofSightCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, maxDistance, playerMask);
        Debug.DrawRay(transform.position, -transform.right * maxDistance, Color.green);
        if (hit)
        {
            CountDownStarted = false;
            Debug.Log("i see u");
            SetState(AiState.Chase);
            

        }
        else
        {
            CountDownStarted = true;

        }
    }

    public void PatrolRoutine()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, patrolPointList[patrolPointIndex], movespeed * Time.deltaTime);
        rb.MovePosition(pos);
        remainingDistance = Vector3.Distance(gameObject.transform.position, patrolPointList[patrolPointIndex]);
        transform.right = (patrolPointList[patrolPointIndex] - transform.position).normalized *-1;
        if (remainingDistance < 0.5f)
        {
            patrolPointIndex++;
            if (patrolPointIndex >= patrolPointList.Count)
            {
                patrolPointIndex = 0;
            }

        }
    }

    public void ChaseRoutine()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, Player.transform.position, movespeed * Time.deltaTime);
        //look at
        rb.MovePosition(pos);
        transform.right = Player.transform.position - transform.position;
    }

    public void DieRoutine()
    {
        Destroy(gameObject, 1f);
    }
}
