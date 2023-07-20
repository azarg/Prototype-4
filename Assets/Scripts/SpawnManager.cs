using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyXPrefab;
    public GameObject powerupPrefab;

    private PlayerController playerController;

    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        //for (int j = 0; j < 10000; j++)
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        int x = Random.Range(0, 100);
        //        Debug.Log(x < (80 + i * 10) && x < 97);
        //        //Debug.Log(x > 97);
        //    }
        //}
    }

    private void StartWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab;
            int x = Random.Range(0, 100);
            // first enemy has 30% chance of being X
            // second enemy has 20% chance of being X
            // third enemy has 10% chance of being X
            // remaining enemies have 10% chance
            prefab = x > 80 ? enemyXPrefab : enemyPrefab;

            Instantiate(prefab, RandomPos(), Quaternion.identity);
        }
    }

    private Vector3 RandomPos()
    {
        var x = Random.Range(-spawnRange, spawnRange);
        var z = Random.Range(-spawnRange, spawnRange);
        return new Vector3(x,0,z);
    }
    // Update is called once per frame
    void Update()
    {
        if (playerController.gameOver)
        {
            var enemies = FindObjectsOfType<EnemyController>();
            for (int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i].gameObject);
                waveNumber = 1;
                playerController.gameOver = false;
            }
            return;
        }

        enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (enemyCount == 0) 
        {
            StartWave(waveNumber);
            waveNumber++;
            //Instantiate(powerupPrefab, RandomPos(), Quaternion.identity);
        }
    }
}
