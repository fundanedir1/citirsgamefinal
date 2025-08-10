using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint; // Sol alt köşe pozisyonu
    public float spawnInterval = 3f;
    private float spawnTimer;

    public List<GameObject> enemies = new List<GameObject>();

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }

        // Ölü düşmanları listeden çıkar
        enemies.RemoveAll(item => item == null);
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemies.Add(enemy);
    }
}
