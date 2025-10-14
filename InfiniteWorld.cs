using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureSpawnData
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnChance = 0.1f;
    public float offsetRange = 0.4f; // Hvor langt unna tile-senteret den kan spawne
}

public class InfiniteWorld : MonoBehaviour
{
    [Header("Prefabs - Tiles")]
    public GameObject tile1;
    public GameObject tile2;
    public GameObject player;

    [Header("Structure Settings")]
    public List<StructureSpawnData> structures = new List<StructureSpawnData>();

    [Header("World Settings")]
    public int viewDistance = 10;
    public float tileSize = 1f;

    private Vector2Int lastPlayerTilePos;
    private Dictionary<Vector2Int, GameObject> spawnedTiles = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        UpdateTiles(force: true);
    }

    void Update()
    {
        UpdateTiles();
    }

    void UpdateTiles(bool force = false)
    {
        if (player == null) return;

        Vector2Int playerTilePos = new Vector2Int(
            Mathf.RoundToInt(player.transform.position.x / tileSize),
            Mathf.RoundToInt(player.transform.position.y / tileSize)
        );

        if (!force && playerTilePos == lastPlayerTilePos) return;
        lastPlayerTilePos = playerTilePos;

        // Spawn nye tiles innenfor viewDistance
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int tilePos = new Vector2Int(playerTilePos.x + x, playerTilePos.y + y);
                if (!spawnedTiles.ContainsKey(tilePos))
                {
                    SpawnTile(tilePos);
                }
            }
        }

        // Fjern tiles som er for langt unna
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kvp in spawnedTiles)
        {
            Vector2Int pos = kvp.Key;
            int dx = Mathf.Abs(pos.x - playerTilePos.x);
            int dy = Mathf.Abs(pos.y - playerTilePos.y);
            if (dx > viewDistance + 2 || dy > viewDistance + 2)
            {
                Destroy(kvp.Value);
                toRemove.Add(pos);
            }
        }

        foreach (var pos in toRemove)
        {
            spawnedTiles.Remove(pos);
        }
    }

    void SpawnTile(Vector2Int gridPos)
    {
        // Velg tilfeldig tile
        GameObject prefabToSpawn = Random.value > 0.5f ? tile1 : tile2;
        Vector3 worldPos = new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, 0);
        GameObject newTile = Instantiate(prefabToSpawn, worldPos, Quaternion.identity, transform);

        // Mulig structure spawn
        TrySpawnStructure(worldPos, newTile.transform);

        spawnedTiles.Add(gridPos, newTile);
    }

    void TrySpawnStructure(Vector3 tileWorldPos, Transform parent)
    {
        foreach (var s in structures)
        {
            if (s.prefab == null) continue;
            if (Random.value < s.spawnChance)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-s.offsetRange, s.offsetRange),
                    Random.Range(-s.offsetRange, s.offsetRange),
                    0f
                );

                Vector3 spawnPos = tileWorldPos + offset;
                Instantiate(s.prefab, spawnPos, Quaternion.identity, parent);
            }
        }
    }
}
