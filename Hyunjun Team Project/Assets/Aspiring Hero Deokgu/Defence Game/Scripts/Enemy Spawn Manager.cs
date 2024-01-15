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
        if (!DefenceGameManager.Instance.IsPlaying())
            return;

        gameTime += Time.deltaTime;

        if( gameTime >= spawnTime )
        {
            float randomEnemy = Random.Range(0.0f, 1.0f);

            int randomspawnPos = Random.Range(0, 5);
            Quaternion quat = Quaternion.Euler(new Vector3(0, -90.0f, 0));

            if( randomEnemy >= 0.4f )
            {
                Instantiate(goblin, spawnPosList[randomspawnPos].position, quat);
            }
            else if( randomEnemy >= 0.1f)
            {
                Instantiate(orc, spawnPosList[randomspawnPos].position, quat);
            }
            else if (randomEnemy >= 0.0f)
            {
                Instantiate(troll, spawnPosList[randomspawnPos].position, quat);
            }

            gameTime = 0.0f;
            spawnTime = Random.Range(0.5f, 3.5f);
        }

    }
}
