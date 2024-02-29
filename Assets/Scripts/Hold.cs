using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Debug.Log("Reparenting Mirror...");
                Mirror.gameObject.layer = Player.gameObject.GetComponent<PlayerController>().originalLayer;
                Mirror.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                Mirror.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Mirror.gameObject.transform.SetParent(Player.transform);
                Mirror.transform.localPosition = new Vector3(0.76f, 0.5f, 0);
            }
        }


    }
}
