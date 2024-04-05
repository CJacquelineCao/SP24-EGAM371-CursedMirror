using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Sprite depletedSprite;
    public bool saved;
    public GameObject particles;

    public AudioSource saveSound;
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
            if(saved == false)
            {
                collision.GetComponent<PlayerController>().setnewspawn(this.gameObject.transform.position);
                gameObject.GetComponent<SpriteRenderer>().sprite = depletedSprite;
                particles.SetActive(false);
                saveSound.Play();
                saved = true;

            }

            
        }
    }
}
