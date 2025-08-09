using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monsterPrefab;
    public Transform[] spawnPoints;
    public int maxDivisionCount = 1;
    public float spawnInterval = 5f;

    [Header("Object Pooling")]
    public int poolSize = 20;

    private float spawnTimer;
    private List<GameObject> pool;

    void Awake()
    {
        // Object Pool oluştur
        pool = new List<GameObject>();

        if (monsterPrefab != null)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(monsterPrefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
    }

    void Start()
    {
        spawnTimer = spawnInterval;

        // Debug kontrolleri
        Debug.Log($"MonsterSpawner başlatıldı. Pool size: {poolSize}");
        Debug.Log($"Spawn points sayısı: {spawnPoints.Length}");

        if (monsterPrefab == null)
            Debug.LogError("Monster prefab atanmamış!");

        if (spawnPoints.Length == 0)
            Debug.LogError("Spawn points boş!");
        else
            Debug.Log($"Pool başarıyla oluşturuldu: {pool.Count} monster");
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            Debug.Log("Spawn zamanı geldi, monster spawn ediliyor...");
            SpawnMonster();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnMonster()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("Spawn noktası yok!");
            return;
        }

        if (monsterPrefab == null)
        {
            Debug.LogError("Monster prefab null!");
            return;
        }

        // Pool'dan monster al
        GameObject monster = GetPooledObject();
        if (monster == null)
        {
            Debug.LogWarning("Pool'dan monster alınamadı!");
            return;
        }

        // Rastgele spawn point seç
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        if (spawnPoint == null)
        {
            Debug.LogError("Seçilen spawn point null!");
            return;
        }

        // Monster'ı konumlandır ve aktif hale getir
        monster.transform.position = spawnPoint.position;
        monster.transform.rotation = Quaternion.identity;
        monster.SetActive(true);

        Debug.Log($"Monster pool'dan spawn edildi: {spawnPoint.position}");
        Debug.Log($"Pool durumu - Aktif monster sayısı: {GetActiveMonsterCount()}");
    }

    public GameObject GetPooledObject()
    {
        // Pool'da pasif obje ara (null check ile)
        foreach (GameObject obj in pool)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Pool doluysa yeni obje oluştur (dinamik genişletme)
        Debug.Log("Pool dolu, yeni monster oluşturuluyor...");
        GameObject newObj = Instantiate(monsterPrefab);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }

    // Aktif monster sayısını hesapla (null check ile)
    int GetActiveMonsterCount()
    {
        int count = 0;
        foreach (GameObject obj in pool)
        {
            if (obj != null && obj.activeInHierarchy)
                count++;
        }
        return count;
    }

    // Monster öldüğünde pool'a geri döndürmek için
    public void ReturnMonsterToPool(GameObject monster)
    {
        if (monster != null && pool.Contains(monster))
        {
            monster.SetActive(false);
            Debug.Log($"Monster pool'a döndürüldü. Aktif monster: {GetActiveMonsterCount()}");
        }
    }

    // Pool durumunu ekranda göster (null check ile)
    void OnGUI()
    {
        if (Application.isPlaying)
        {
            int activeCount = GetActiveMonsterCount();
            int totalValidObjects = 0;

            // Null olmayan objeleri say
            foreach (GameObject obj in pool)
            {
                if (obj != null)
                    totalValidObjects++;
            }

            int pooledCount = totalValidObjects - activeCount;

            GUI.Label(new Rect(10, 10, 250, 20), $"Aktif Monster: {activeCount}");
            GUI.Label(new Rect(10, 30, 250, 20), $"Pool'da Bekleyen: {pooledCount}");
            GUI.Label(new Rect(10, 50, 250, 20), $"Toplam Geçerli: {totalValidObjects}");
        }
    }
}