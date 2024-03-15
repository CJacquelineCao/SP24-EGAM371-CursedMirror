using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hold : MonoBehaviour
{
    public Transform Mirror;
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Mirror.gameObject.transform.parent != gameObject.transform)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                if (Player.gameObject.GetComponent<PlayerController>().canPickUp == true)
                {
                    Debug.Log("Reparenting Mirror...");
                    Mirror.gameObject.layer = Player.gameObject.GetComponent<PlayerController>().originalLayer;
                    Player.gameObject.GetComponent<PlayerController>().ChangeMirrorLayer("Short");
                    Mirror.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    Mirror.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    Mirror.gameObject.transform.SetParent(Player.transform);
                    Mirror.transform.localPosition = new Vector3(0.76f, 0.5f, 0);
                    Player.gameObject.GetComponent<PlayerController>().Tstrength = 0;
                    Player.gameObject.GetComponent<PlayerController>().strengthSlider.GetComponent<Slider>().value = 0;
                    Player.gameObject.GetComponent<PlayerController>().canThrow = true;
                    Player.gameObject.GetComponent<PlayerController>().canPickUp = false;
                }

            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if(collision.tag == "Ground")
        {
            Mirror.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Player.gameObject.GetComponent<PlayerController>().canPickUp = true;
            //it seems like... when its super short, it runs into issues??
        }


    }
}
