using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunlight : MonoBehaviour
{
    private LineRenderer lightRenderer;
    public float lightDistance;
    public int numberReflections;
    public int currentReflects;

    private Vector3 pos;
    private Vector3 dirLight;

    public Transform Mirror;
    public bool loopActive;

    public LayerMask reflection;

    // Start is called before the first frame update
    void Start()
    {
        lightRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        NormalState();
    }

    public void NormalState()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, lightDistance, reflection);
        Debug.DrawRay(transform.position, -transform.up * lightDistance, Color.green);
        lightRenderer.SetPosition(0, transform.position);
        if (hit)
        { 
            dirLight = Vector3.Reflect(transform.position, hit.normal);
            Debug.DrawRay(hit.point, dirLight, Color.green);

            lightRenderer.positionCount = 3;
            lightRenderer.SetPosition(1, hit.point);
            Vector3 continuePos = (dirLight.normalized * lightDistance) - dirLight;
            lightRenderer.SetPosition(2, continuePos);


            Debug.Log("Reflecting");
        }
        else
        {
            Debug.Log("there's nothing");
            Vector3 DownPos = -transform.up*lightDistance;
            lightRenderer.positionCount = 2;
            lightRenderer.SetPosition(1, DownPos);

        }
    }
    // gotta find a way to make this more modular...incorporate the current reflects int without it counting the same raycast again.
    //the loop idea sounds good...that way the light can reflect off multiple objects...when it hits the maximum amount of items, it breaks out of the loop
    void Shine()
    {

        dirLight = Mirror.position - pos;

        while(loopActive)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, dirLight, lightDistance, 7);
            if(hit)
            {
                currentReflects++;
                lightRenderer.positionCount = currentReflects;
                dirLight = Vector3.Reflect(dirLight, hit.normal);
                pos = (Vector2)dirLight.normalized+hit.point;
                lightRenderer.SetPosition(currentReflects - 1, hit.point);
            }
            else
            {
                lightRenderer.positionCount = currentReflects;
                lightRenderer.SetPosition(currentReflects - 1, pos + (dirLight.normalized * lightDistance));
                //loopActive = false;
            }
            if(currentReflects > numberReflections)
            {
                loopActive = false;
            }
            
        }

    }

}
