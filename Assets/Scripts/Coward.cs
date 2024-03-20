using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward : MonoBehaviour
{
    public GameObject Player;

    public float runSpeed = 5f;
    public float runawayDistance = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<GenericEnemy>().currentState == GenericEnemy.AiState.Special)
        {
            runAway();
        }
    }

    public void runAway()
    {
        // Calculate direction away from the player
        Vector3 directionToPlayer = transform.position - Player.transform.position;

        // Normalize the direction vector
        directionToPlayer.Normalize();

        // Calculate the position to move to (current position + direction * runawayDistance)
        Vector3 targetPosition = transform.position + directionToPlayer * runawayDistance;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
    }
}
