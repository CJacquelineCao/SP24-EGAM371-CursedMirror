using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ugly : MonoBehaviour
{
    public GameObject Player;
    public LayerMask playerMask;
    public float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        lineofSightCheck();   
    }
    public void lineofSightCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, maxDistance, playerMask);
        Debug.DrawRay(transform.position, -transform.right * maxDistance, Color.green);
        if (hit)
        {
            if(Player.GetComponent<PlayerController>().MirrorOut == true)
            {
                Player.GetComponent<PlayerController>().MirrorBroke = true;
            }            



        }
        else
        {

        }
    }

}
