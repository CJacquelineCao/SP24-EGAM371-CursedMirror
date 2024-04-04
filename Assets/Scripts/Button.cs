using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Door matchingDoor;
    public GameObject designatedObject;

    public bool inverted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inverted == true)
        {
            if (designatedObject == null)
            {
                Lock();
            }
        }
        else
        {
            if (designatedObject == null)
            {
                Unlock();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inverted == true)
        {
            if (collision.tag == designatedObject.transform.tag)
            {
                Unlock();
            }
            else if(collision.tag == "Player")
            {
                Unlock();
            }
        }
        else
        {
            if (collision.tag == designatedObject.transform.tag)
            {
                Lock();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(inverted == true)
        {
            Lock();
        }
        else
        {
            if (collision.tag != "Shard")
            {
                if (collision.tag == designatedObject.transform.tag)
                {
                    Unlock();
                }

            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision ==null)
        {
            if(inverted == true)
            {
                Lock();
            }
            else
            {
                Unlock();
            }

        }
    }
    public void Unlock()
    {
        matchingDoor.Locked = false;
        matchingDoor.doorunlockSound.Play();
    }

    public void Lock()
    {
        matchingDoor.Locked = true;
    }
}
