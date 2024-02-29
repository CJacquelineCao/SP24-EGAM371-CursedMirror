using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 movedirection;
    public int movespeed;
    public Transform Mirror;

    public Sprite MirrorBack;
    public Sprite MirrorFront;
    public Sprite MirrorBroken;

    public bool MirrorBroke;
    public GameObject[] Hearts;
    public int life;
    public bool isRight;

    public float Tstrength;

    public GameObject ShardPrefab;
    public GameObject currentShard;

    public float holdTime;
    public float increaseInterval;

    public int originalLayer;

    Vector2 initialPosition;
    public float maxDistance;

    public bool MirrorOut;
    // Start is called before the first frame update
    void Start()
    {
        originalLayer = Mirror.gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        move();
        if (life < 1)
        {
            Hearts[0].gameObject.SetActive(false);
            

        }
        else if (life < 2)
        {
            Hearts[1].gameObject.SetActive(false);
        }
        else if (life < 3)
        {
            Hearts[2].gameObject.SetActive(false);

        }
        if (Input.GetAxis("Horizontal") < 0 && isRight == true)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.GetChild(0).localScale = new Vector3(-transform.GetChild(0).transform.localScale.x, -transform.GetChild(0).transform.localScale.y, 1);
            isRight = false;
        }
        else if (Input.GetAxis("Horizontal") > 0 && isRight == false)
        {
            transform.localScale = new Vector3(1, 1, 1); 
            transform.GetChild(0).localScale = new Vector3(transform.GetChild(0).transform.localScale.x, transform.GetChild(0).transform.localScale.y, 1);
            isRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.tag == "Enemy")
        {
            life -= collision.gameObject.GetComponent<GenericEnemy>().AttackPower;
        }

    }

    
    

    private void FixedUpdate()
    {
        if(MirrorBroke == false)
        {
            if (Input.GetMouseButton(1))
            {
                MirrorOut = true;
                Mirror.gameObject.SetActive(true);
                Mirror.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                Mirror.gameObject.GetComponent<SpriteRenderer>().sprite = MirrorFront;
                Vector2 Mirdir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Mirror.position).normalized;
                Debug.Log(Mirdir);
                Mirror.GetComponent<Rigidbody2D>().rotation = GetAngleFromVectorFloat(Mirdir);

            }
            else if (Input.GetKey(KeyCode.Space))
            {
                MirrorOut = false;
                Mirror.gameObject.SetActive(true);
                Mirror.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                Mirror.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

                if (Tstrength < maxDistance)
                {
                    Debug.Log("Charging...");
                    holdTime += Time.deltaTime;
                    if (holdTime >= increaseInterval)
                    {
                        Tstrength += 1;
                        holdTime = 0f;
                    }
                }

            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                throwMirror();

            }
            else
            {
                if(Mirror.gameObject.transform.parent == gameObject.transform)
                {
                    MirrorOut = false;
                    Mirror.gameObject.GetComponent<SpriteRenderer>().sprite = MirrorBack;
                    Mirror.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }

                //Mirror.gameObject.SetActive(false);


            }

        }
        else
        {
            Mirror.gameObject.GetComponent<SpriteRenderer>().sprite = MirrorBroken;
            MirrorOut = false;
            if(GameObject.FindObjectOfType<Shatter>() == null)
            {
                currentShard = Instantiate(ShardPrefab, Mirror.gameObject.transform.position, Quaternion.identity);
            }
            else
            {
            }

        }
      

    }

    public void throwMirror()
    {
        initialPosition = Mirror.position;
        Vector2 Throwdir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Mirror.position).normalized;
        Mirror.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        ChangeMirrorLayer();
        Mirror.gameObject.GetComponent<Rigidbody2D>().AddForce(Throwdir * 100, ForceMode2D.Impulse);
        Mirror.gameObject.transform.parent = null;

        holdTime = 0f;
        //find a way to switch it back to kinematic but also make it stop movingit
        StartCoroutine(CheckDistance());
    }
    IEnumerator CheckDistance()
    {
        yield return new WaitForSeconds(0.5f); 

        float distanceTraveled = Vector2.Distance(initialPosition, Mirror.position);
       

        if (distanceTraveled >= Tstrength)
        {
            Mirror.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            Mirror.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Tstrength = 0;

        }
    }
    void ChangeMirrorLayer()
    {

        int temporaryLayer = LayerMask.NameToLayer("Shard");
        Mirror.gameObject.layer = temporaryLayer;
    }
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;

    }
    void move()
    {

        rb.velocity = new Vector2(movedirection.x * movespeed, movedirection.y * movespeed);
    }
    void Inputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movedirection = new Vector2(moveX, moveY).normalized;
    }
}
