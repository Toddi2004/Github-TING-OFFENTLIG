using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public int baseHealth = 50;
    private int currentHealth;

    [Header("References")]
    public EnemyDrops enemyDrops;

    void Start()
    {
        // Hent vanskelighetsverdi fra DifficultyManager
        float diff = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficultyValue() : 1f;

        // Skaler HP additivt med difficulty
        currentHealth = Mathf.RoundToInt(baseHealth + diff * 5f); // juster 5f for hvor mye diff påvirker HP
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (enemyDrops != null)
        {
            enemyDrops.DropLoot();
        }

        Destroy(gameObject);
    }
}
