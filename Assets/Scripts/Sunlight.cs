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

    public EnemyController enemyref;
    public PlayerController playerref;

    // Start is called before the first frame update
    void Start()
    {
        lightRenderer = GetComponent<LineRenderer>();
        playerref = GameObject.FindAnyObjectByType<PlayerController>();
        enemyref = GameObject.FindAnyObjectByType<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        NormalState();
    }

    public void NormalState()
    {
        Vector2 RayStart = transform.position;
        Vector2 RayDir = -transform.up;
        float DistanceRemaining = lightDistance;
        List<Vector3> linePoints = new List<Vector3>(); 
        for(int i=0; i<numberReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(RayStart, RayDir, DistanceRemaining, reflection);
            
            linePoints.Add(RayStart);
            if (hit)
            {
                if (hit.transform.tag == "Enemy")
                {
                    enemyref.detectEnemyDeath(hit.transform.gameObject);
                    linePoints.Add(RayStart + (RayDir * DistanceRemaining));
                    Debug.Log("HitEnemy");
                    break;
                }
                else if(hit.transform.tag == "Ground")
                {
                    linePoints.Add(hit.point);
                    break;
                }

                else if(hit.transform.tag == "Lock")
                {
                    hit.transform.gameObject.GetComponent<SunLock>().Unlock();
                    linePoints.Add(hit.point);
                    break;
                }
                else if(hit.transform.tag == "Rope")
                {
                    hit.transform.gameObject.GetComponent<Rope>().burn();
                    linePoints.Add(hit.point);
                    break;
                }
                else if(hit.transform.tag == "Mirror")
                {
                    RayDir = hit.transform.right;
                    Debug.DrawLine(RayStart, hit.point, Color.green);
                    DistanceRemaining -= hit.distance;
                    RayStart = hit.point + (RayDir * 0.5f);

                }
                else
                {
                    RayDir = Vector3.Reflect(RayDir, hit.normal);
                    Debug.DrawLine(RayStart, hit.point, Color.green);
                    DistanceRemaining -= hit.distance;
                    RayStart = hit.point + (hit.normal * 0.01f);



                    Debug.Log("Reflecting off of" + hit.transform.name, hit.transform);
                    
                }
                if(DistanceRemaining < .7)
                {
                    linePoints.Add(hit.point);
                    break;
                }
            }
            else
            {
                Debug.Log("there's nothing");
                //playerref.underLight = false;
                linePoints.Add(RayStart + (RayDir * DistanceRemaining));
                break;

            }
        }
        lightRenderer.positionCount = linePoints.Count;
        lightRenderer.SetPositions(linePoints.ToArray());

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
