using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    PlayerController playerref;
    SideScrollMovement sideScrollRef;
    // Start is called before the first frame update
    void Start()
    {
        playerref = GameObject.FindObjectOfType<PlayerController>();
        sideScrollRef = GameObject.FindObjectOfType<SideScrollMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentSceneNum = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneNum ==0)
        {
            if (playerref.life <= 0)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }
        else
        {
            if(sideScrollRef.life <=0)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }


        if(Input.GetKeyDown(KeyCode.B))
        {

            if(currentSceneNum == 0)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
