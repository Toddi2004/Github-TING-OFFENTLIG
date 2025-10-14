using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [Header("Drop Settings")]
    public GameObject goldPrefab;
    public GameObject xpPrefab;
    public int goldAmount = 1;
    public int xpAmount = 1;

    [Header("Drop Chance")]
    [Range(0f, 1f)] public float goldDropChance = 0.5f;
    [Range(0f, 1f)] public float xpDropChance = 1f;

    [Header("Score")]
    public int scoreValue = 10;

    [Header("Difficulty Increase")]
    [Tooltip("Hvor mye vanskeligheten øker når denne fienden dør")]
    public float difficultyIncrease = 0.5f;

    [Header("Debug")]
    public bool debugLog = false;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = Object.FindFirstObjectByType<PlayerStats>();
    }

    public void DropLoot()
    {
        if (playerStats != null)
            playerStats.AddScore(scoreValue);

        Vector3 basePos = new Vector3(transform.position.x, transform.position.y, 0f);

        // XP
        if (xpPrefab != null && Random.value <= xpDropChance)
        {
            for (int i = 0; i < xpAmount; i++)
            {
                Vector2 offset = Random.insideUnitCircle * 0.3f;
                Vector3 pos = basePos + new Vector3(offset.x, offset.y, 0f);
                Instantiate(xpPrefab, pos, Quaternion.identity);
            }
            if (debugLog) Debug.Log($"[EnemyDrops] Dropped {xpAmount} XP at {basePos}");
        }

        // Gold
        if (goldPrefab != null && Random.value <= goldDropChance)
        {
            for (int i = 0; i < goldAmount; i++)
            {
                Vector2 offset = Random.insideUnitCircle * 0.3f;
                Vector3 pos = basePos + new Vector3(offset.x, offset.y, 0f);
                Instantiate(goldPrefab, pos, Quaternion.identity);
            }
            if (debugLog) Debug.Log($"[EnemyDrops] Dropped {goldAmount} Gold at {basePos}");
        }

        // Øk vanskelighet spesifikt for denne fienden
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.AddDifficulty(difficultyIncrease);
            if (debugLog)
                Debug.Log($"[EnemyDrops] Increased difficulty by {difficultyIncrease}");
        }
    }
}
