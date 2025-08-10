using UnityEngine;
using UnityEngine.Tilemaps;

public class IsoMonster : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public Tilemap tilemap;
    public float moveInterval = 1f;  // Kaç saniyede bir yön değiştirecek
    public float moveSpeed = 2f;

    private Vector3Int currentCell;
    private Vector3 targetPos;
    private float moveTimer;
    private BoundsInt mapBounds;
    private Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),    // sağ
        new Vector3Int(-1, 0, 0),   // sol
        new Vector3Int(0, 1, 0),    // yukarı
        new Vector3Int(0, -1, 0),   // aşağı
        new Vector3Int(1, 1, 0),    // sağ-yukarı
        new Vector3Int(-1, 1, 0),   // sol-yukarı
        new Vector3Int(1, -1, 0),   // sağ-aşağı
        new Vector3Int(-1, -1, 0)   // sol-aşağı
    };

    [Header("Yaşam Süresi Ayarları")]
    public float baseLifeTime = 10f; // Normal yaşam süresi
    public float extraLifeTime = 0f; // Ödülle artacak süre
    private float lifeTimer;

    private MonsterSpawner spawner;

    void Start()
    {
        // Spawner referansını al
        spawner = FindObjectOfType<MonsterSpawner>();
    }

    void OnEnable()
    {
        // Her spawn olduğunda çağrılır
        if (tilemap == null)
            tilemap = FindObjectOfType<Tilemap>();

        mapBounds = tilemap.cellBounds;
        currentCell = tilemap.WorldToCell(transform.position);
        targetPos = transform.position;
        moveTimer = moveInterval;

        // Yaşam süresini sıfırla (her spawn'da)
        lifeTimer = baseLifeTime + extraLifeTime;

        Debug.Log($"Monster spawn oldu, yaşam süresi: {lifeTimer} saniye");
    }

    void Update()
    {
        // Hareket
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            PickRandomDirection();
            moveTimer = moveInterval;
        }

        // Yaşam süresi
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Die(); // Destroy yerine Die metodunu çağır
        }
    }

    void PickRandomDirection()
    {
        Vector3Int randomDir = directions[Random.Range(0, directions.Length)];
        Vector3Int nextCell = currentCell + randomDir;

        bool insideBounds = mapBounds.Contains(nextCell);
        bool hasTile = tilemap.HasTile(nextCell);

        if (insideBounds && hasTile)
        {
            currentCell = nextCell;
            targetPos = tilemap.CellToWorld(currentCell) + (Vector3)tilemap.cellSize / 2;
        }
        else
        {
            PickRandomDirection(); // Recursive call - dikkat infinite loop'a
        }
    }

    void Die()
    {
        Debug.Log("Monster ölüyor, pool'a döndürülüyor...");

        // Pool'a geri döndür
        if (spawner != null)
        {
            spawner.ReturnMonsterToPool(gameObject);
        }
        else
        {
            // Fallback - eğer spawner bulunamazsa sadece deaktif et
            gameObject.SetActive(false);
            Debug.LogWarning("MonsterSpawner bulunamadı, monster sadece deaktif edildi!");
        }
    }

    // Harici etkenlerle monster öldürüldüğünde de çağrılabilir
    public void Kill()
    {
        Die();
    }
}