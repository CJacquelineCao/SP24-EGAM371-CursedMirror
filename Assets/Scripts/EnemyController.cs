using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    [System.Serializable]
    public class Enemygeneration
    {
        public int ID;
        public GameObject EnemyRef;
        public bool spawned;
        public Vector3 position;
        public float Spawnspeed;
        public float initaldelay;
        public PatrolPointsData patrolPoints;
    }



    public List<Enemygeneration> EnemyList;

    public Enemy Database;
    // Start is called before the first frame update
    void Start()
    {
        InitializeSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        //ClearEnemyList();

    }

    public IEnumerator spawnSet()
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i].EnemyRef == null && EnemyList[i].spawned == false)
            {
                for(int e =0; e<Database.AllEnemyDB.Count; e++)
                {
                    if(EnemyList[i].ID == Database.AllEnemyDB[e].id)
                    {
                        //this is the correct enemy, lets spawn it
                        yield return new WaitForSeconds(EnemyList[i].initaldelay);
                        Debug.Log("Spawn 1 Enemy");
                        EnemyList[i].EnemyRef = Instantiate(Database.AllEnemyDB[e].enemyPrefab, EnemyList[i].position, Quaternion.identity);
                        EnemyList[i].EnemyRef.GetComponent<GenericEnemy>().patrolPointsData = EnemyList[i].patrolPoints;
                        //EnemyList[i].EnemyRef.transform.localScale = new Vector3(EnemyList[i].EnemyRef.transform.localScale.x, EnemyList[i].EnemyRef.transform.localScale.y, EnemyList[i].EnemyRef.transform.localScale.z);
                    }
                }
            }
            else
            {
                //this enemy has already been spawned
            }
        }

        
    }

    public void InitializeSpawn()
    {
        StartCoroutine(spawnSet());

    }

    public void detectEnemyDeath(GameObject enemy)
    {
        for(int i=0; i< EnemyList.Count; i++)
        {
            if(enemy == EnemyList[i].EnemyRef)
            {
                if(EnemyList[i].ID == 0)
                {
                    EnemyList[i].EnemyRef.GetComponent<GenericEnemy>().SetState(GenericEnemy.AiState.Die);
                }

                
                // is there a way to find all enemies? separate out the script? but then the AI state doesnt work hmm
            }
        }

    }


}
