using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public float maxtimeneeded;

    public float lighttime;
    public float currenttime;

    public float burnspeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currenttime<=maxtimeneeded)
        {
            if(currenttime >=3)
            {

                gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                Destroy(gameObject, 1f);
            }
            else if(currenttime >2)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (currenttime > 1)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
            else if (currenttime > 0.5)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                // regularcolor
            }
        }
        else
        {
            
        }
    }

    public void burn()
    {
        if (currenttime < maxtimeneeded)
        {
            lighttime += Time.deltaTime;
            if (lighttime >= burnspeed)
            {
               currenttime += 0.5f;
                lighttime = 0f;
              
            }
        }
    }
    public void stopburn()
    {
        currenttime = 0;
    }
}
