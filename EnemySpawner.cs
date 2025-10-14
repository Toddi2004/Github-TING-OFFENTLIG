using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;        // Prefab for fienden
    public Transform player;              // Referanse til spilleren

    [Header("Spawn Settings")]
    public int baseEnemiesPerWave = 3;    // Hvor mange fiender i starten
    public float spawnInterval = 3f;      // Hvor ofte det spawner fiender (sekunder)
    public float spawnRadius = 10f;       // Hvor langt unna spilleren de spawner
    public float randomOffset = 1.5f;     // Hvor mye variasjon det kan være rundt radiusen

    [Header("Difficulty Scaling (additiv)")]
    public float enemiesPerDifficulty = 1.0f; // Hvor mye antallet øker per difficulty-verdi

    private float nextSpawnTime;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null || enemyPrefab == null) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemies();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemies()
    {
        // Hent additiv vanskelighetsverdi
        float diff = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficultyValue() : 1f;

        // Beregn antall fiender basert på additiv difficulty
        int enemiesToSpawn = Mathf.RoundToInt(baseEnemiesPerWave + diff * enemiesPerDifficulty);
        enemiesToSpawn = Mathf.Max(0, enemiesToSpawn);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Velg et tilfeldig punkt på en sirkel rundt spilleren
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = spawnRadius + Random.Range(-randomOffset, randomOffset);

            Vector2 spawnPos = (Vector2)player.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            // Sørg for at fiender ikke spawner for tett på spilleren
            if (Vector2.Distance(spawnPos, player.position) < 2f)
            {
                spawnPos = (Vector2)player.position + (spawnPos - (Vector2)player.position).normalized * 2f;
            }

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }

        Debug.Log($"Spawned {enemiesToSpawn} enemies at difficulty {diff:F2}");
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Tegn spawnradius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, spawnRadius);

        // Tegn variasjonsgrense (offset)
        if (randomOffset > 0)
        {
            Gizmos.color = new Color(1, 0.5f, 0, 0.5f);
            Gizmos.DrawWireSphere(player.position, spawnRadius + randomOffset);
            Gizmos.DrawWireSphere(player.position, spawnRadius - randomOffset);
        }
    }
}
