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
        new Vector3Int(5, 0, 0),    // sağ
        new Vector3Int(-1, 0, 0),   // sol
        new Vector3Int(0, 1, 0),    // yukarı
        new Vector3Int(0, -1, 0),   // aşağı
        new Vector3Int(1, 1, 0),    // sağ-yukarı
        new Vector3Int(-1, 1, 0),   // sol-yukarı
        new Vector3Int(1, -1, 0),   // sağ-aşağı
        new Vector3Int(-1, -1, 0)   // sol-aşağı
    };

    void OnEnable()
    {
        if (tilemap == null)
            tilemap = FindObjectOfType<Tilemap>();

        mapBounds = tilemap.cellBounds;

        currentCell = tilemap.WorldToCell(transform.position);
        targetPos = transform.position;
        moveTimer = moveInterval;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            PickRandomDirection();
            moveTimer = moveInterval;
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
            PickRandomDirection();
        }
    }
}
