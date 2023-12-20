using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public List<Transform> spawnPosList = new List<Transform>();
    public GameObject goblin;
    public GameObject orc;
    public GameObject troll;

    GameObject goal;
    float gameTime = 0.0f;
    float spawnTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.Find("Goal");
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }
    
    void SpawnEnemy()
    {
        if (!DefenceGameManager.instance.IsPlaying())
            return;

        gameTime += Time.deltaTime;

        if( gameTime >= spawnTime )
        {
            int randomEnemy = Random.Range(1, 4);

            int randomspawnPos = Random.Range(0, 5);
            Quaternion quat = Quaternion.Euler(new Vector3(0, -90.0f, 0));

            switch (randomEnemy)
            {
                case 1:
                    Instantiate(goblin, spawnPosList[randomspawnPos].position, quat);
                    break;
                case 2:
                    Instantiate(orc, spawnPosList[randomspawnPos].position, quat);
                    break;
                case 3:
                    Instantiate(troll, spawnPosList[randomspawnPos].position, quat);
                    break;
                default:
                    break;
            }

            gameTime = 0.0f;
            spawnTime = Random.Range(0.5f, 3.0f);
        }

    }
}
