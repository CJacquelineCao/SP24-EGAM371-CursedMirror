using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Door matchingDoor;
    public GameObject designatedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(designatedObject == null)
        {
            Unlock();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Shard")
        {
            if(collision.tag == designatedObject.transform.tag)
            {
                Unlock();
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision ==null)
        {
            Unlock();
        }
    }
    public void Unlock()
    {
        matchingDoor.Locked = false;
    }
}
