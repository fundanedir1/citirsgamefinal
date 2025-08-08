using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    public ObjectPooler objectPooler;   // Object pool referansı
    public Transform[] spawnPoints;     // Spawn noktaları
    public float spawnInterval = 1f;    // Kaç saniyede bir spawn

    private float spawnTimer;

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnMonster();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnMonster()
    {
        // Rastgele bir spawn noktası seç
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Havuzdan bir canavar objesi al
        GameObject monster = objectPooler.GetPooledObject();

        if (monster != null)
        {
            monster.transform.position = spawnPoint.position;
            monster.transform.rotation = Quaternion.identity;
            monster.SetActive(true);

            // Tilemap referansını ver (eğer yoksa)
            IsoMonster isoMonster = monster.GetComponent<IsoMonster>();
            if (isoMonster != null && isoMonster.tilemap == null)
            {
                isoMonster.tilemap = FindObjectOfType<Tilemap>();
            }
        }
    }
}
