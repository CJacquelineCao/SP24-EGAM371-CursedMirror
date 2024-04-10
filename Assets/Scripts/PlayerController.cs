using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    public LineRenderer aimLine;
    public float lineLength;
    public bool canThrow;
    public GameObject strengthSlider;
    public bool canPickUp;

    public float throwstrengthfactor;

    public bool canMove;
    public Vector3 respawnLocation;
    public AudioSource breakSound;
    public bool invincibilitycalled;
    public bool isInvincible;
    public Animator animationcontroller;
    // Start is called before the first frame update
    void Start()
    {
        originalLayer = Mirror.gameObject.layer;
        canMove = true;

        // Disable the Line Renderer at the start
        aimLine.enabled = false;

        respawnLocation = new Vector3(-45, -3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            Inputs();
            move();
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
            TakeDmg(1);
        }
        else if(collision.transform.tag == "Bullet")
        {
            TakeDmg(1);
            Destroy(collision.gameObject); // Destroy the bullet on hitting the player
        }

    }

    public void TakeDmg(int dmg)
    {
        if (isInvincible == false)
        {
            life -= dmg;
            if (life <= 0)
            {
                //gameover
            }
            else
            {

                for (int i = 0; i < Hearts.Length; i++)
                {
                    if (Hearts[i].activeSelf == true)
                    {
                        Hearts[i].gameObject.SetActive(false);
                        break;
                    }
                }
                if (invincibilitycalled == false)
                {
                    StartCoroutine(beInvincible());
                }

            }
        }
        else
        {

        }

    }
    IEnumerator beInvincible()
    {
        invincibilitycalled = true;
        animationcontroller.SetBool("Hurt", true);
        yield return new WaitForSeconds(.1f);
        isInvincible = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2.15f);
        isInvincible = false;
        animationcontroller.SetBool("Hurt", false);
        gameObject.GetComponent<Collider2D>().enabled = true;
        invincibilitycalled = false;
    }

    private void FixedUpdate()
    {
        if(MirrorBroke == false)
        {
            if (Input.GetMouseButton(1))
            {
                if (canThrow == true)
                {
                    MirrorOut = true;
                    Mirror.gameObject.GetComponent<Collider2D>().enabled = true;
                    Mirror.gameObject.GetComponent<SpriteRenderer>().sprite = MirrorFront;
                    Vector2 Mirdir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Mirror.position).normalized;
                    Debug.Log(Mirdir);
                    Mirror.GetComponent<Rigidbody2D>().rotation = GetAngleFromVectorFloat(Mirdir);

                }
            }

            else if (Input.GetKey(KeyCode.Space) && canThrow == true)
            {
                canMove = false;
                MirrorOut = false;
                strengthSlider.SetActive(true);
                aimLine.enabled = true;
                Mirror.gameObject.SetActive(true);
                Mirror.gameObject.GetComponent<Collider2D>().enabled = true;
                Mirror.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

                Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Mirror.position);
                direction.z = 0;

               // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
               //aimLine.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

                aimLine.SetPosition(0, gameObject.transform.position);
                aimLine.SetPosition(1, gameObject.transform.position + direction.normalized * lineLength*Tstrength);



                if (Tstrength < maxDistance)
                {
                    Debug.Log("Charging...");
                    holdTime += Time.deltaTime;
                    if (holdTime >= increaseInterval)
                    {
                        Tstrength += 1;
                        holdTime = 0f;
                        strengthSlider.GetComponent<Slider>().value = Tstrength / maxDistance;
                    }
                }

            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                if(Tstrength ==0)
                {
                    Tstrength = 1;
                }
                aimLine.enabled = false;
                canMove = true;
                throwMirror();

            }
            else
            {
                if(Mirror.gameObject.transform.parent == gameObject.transform)
                {
                    MirrorOut = false;
                    Mirror.gameObject.GetComponent<SpriteRenderer>().sprite = MirrorBack;
                    Mirror.gameObject.GetComponent<Collider2D>().enabled = false;
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
        canPickUp = false;
        initialPosition = Mirror.position;
        Vector2 Throwdir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Mirror.position).normalized;
        Mirror.gameObject.GetComponent<Collider2D>().enabled = true;
        ChangeMirrorLayer("Shard");
        Mirror.gameObject.GetComponent<Rigidbody2D>().AddForce(Throwdir.normalized * Tstrength * throwstrengthfactor, ForceMode2D.Impulse);
        Mirror.gameObject.transform.parent = null;

        holdTime = 0f;
        canThrow = false;
        strengthSlider.SetActive(false);
        //find a way to switch it back to kinematic but also make it stop movingit
        StartCoroutine(CheckDistance());
    }
    IEnumerator CheckDistance()
    {
        yield return new WaitForSeconds(0.5f); 

        float distanceTraveled = Vector2.Distance(initialPosition, Mirror.position);
       
        while (canPickUp == false)
        {
            if (Mirror.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 0.5)
            {
                Mirror.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                Mirror.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Mirror.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
                Mirror.gameObject.GetComponent<Collider2D>().enabled = true;
                canPickUp = true;
                Tstrength = 0;

            }
            yield return new WaitForEndOfFrame();
        }

    }
    public void ChangeMirrorLayer(string name)
    {

        int temporaryLayer = LayerMask.NameToLayer(name);
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

    public void setnewspawn(Vector3 pos)
    {
        respawnLocation = pos;
        life = 3;
        foreach (GameObject heart in Hearts)
        {
            heart.SetActive(true);
        }
    }
     void respawn()
    {
        gameObject.transform.position = respawnLocation;
        life = 3;
        foreach (GameObject heart in Hearts)
        {
            heart.SetActive(true);
        }
    }
}
