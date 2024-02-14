using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    PlayerController playerref;
    // Start is called before the first frame update
    void Start()
    {
        playerref = GameObject.FindObjectOfType<PlayerController>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(playerref.life <= 0)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);   
        }
    }
}
