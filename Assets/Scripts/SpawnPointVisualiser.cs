using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways] // Editör modunda da çalışsın
public class SpawnPointVisualizer : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform[] spawnPoints;
    public float gizmoSize = 0.3f;

    private void OnDrawGizmos()
    {
        if (tilemap == null || spawnPoints == null) return;

        foreach (Transform sp in spawnPoints)
        {
            if (sp == null) continue;

            Vector3Int cellPos = tilemap.WorldToCell(sp.position);
            bool isOnTile = tilemap.HasTile(cellPos);

            // Renk seçimi
            Gizmos.color = isOnTile ? Color.green : Color.red;

            // Küre çiz
            Gizmos.DrawSphere(sp.position, gizmoSize);

#if UNITY_EDITOR
            // Etiket yaz
            GUIStyle style = new GUIStyle();
            style.normal.textColor = isOnTile ? Color.green : Color.red;
            style.fontSize = 12;
            Handles.Label(sp.position + Vector3.up * 0.3f, sp.name, style);
#endif
        }
    }
}
