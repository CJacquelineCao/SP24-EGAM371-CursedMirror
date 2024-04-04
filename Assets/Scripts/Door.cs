using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Locked;
    public GameObject collider;

    public AudioSource doorunlockSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Locked == false)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Locked == false)
        {
            if (collision.tag == "Player")
            {
                collider.SetActive(false);
            }

        }
        if (collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }


    }
}
