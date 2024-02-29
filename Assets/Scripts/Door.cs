using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Locked;
    public GameObject collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Locked == false)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
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

    }
}
