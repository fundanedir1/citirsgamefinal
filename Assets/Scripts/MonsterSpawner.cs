using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public ObjectPooler pooler; // ObjectPooler referansı
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;

    [Header("Monster Division")]
    public int maxDivisionCount = 1; // Eski kod uyumluluğu için

    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval;

        if (pooler == null)
            Debug.LogError("MonsterSpawner: ObjectPooler atanmadı!");

        if (spawnPoints.Length == 0)
            Debug.LogError("MonsterSpawner: Spawn noktası yok!");
    }

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
        if (pooler == null || spawnPoints.Length == 0)
            return;

        GameObject monster = pooler.GetPooledObject();
        if (monster == null)
        {
            Debug.LogWarning("Havuzda uygun monster bulunamadı!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        monster.transform.position = spawnPoint.position;
        monster.transform.rotation = Quaternion.identity;
        monster.SetActive(true);

        Debug.Log($"Monster spawn edildi: {spawnPoint.position}");
    }

    // Eski kodlarla uyumlu: Monster'ı havuza geri döndür
    public void ReturnMonsterToPool(GameObject monster)
    {
        if (pooler != null && monster != null)
        {
            pooler.ReturnToPool(monster);
        }
    }
}
