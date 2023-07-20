using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyXPrefab;
    public GameObject[] powerupPrefabs;
    public float chanceToSpawnX;
    private PlayerController playerController;

    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 1;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void StartWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab;
            int x = Random.Range(0, 100);
            prefab = x < (100 * chanceToSpawnX) ? enemyXPrefab : enemyPrefab;

            Instantiate(prefab, RandomPos(), Quaternion.identity);
        }
    }

    private Vector3 RandomPos()
    {
        var x = Random.Range(-spawnRange, spawnRange);
        var z = Random.Range(-spawnRange, spawnRange);
        return new Vector3(x,0,z);
    }

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
            Instantiate(powerupPrefabs[0], RandomPos(), Quaternion.identity);
            Instantiate(powerupPrefabs[1], RandomPos(), Quaternion.identity);
        }
    }
}
