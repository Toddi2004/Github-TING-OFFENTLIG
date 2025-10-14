using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Stats")]
    public float baseDamage = 10f;        // Grunnskade
    public float lifetime = 3f;           // Hvor lenge kulen eksisterer før den ødelegges

    [Header("Movement")]
    public float speed = 10f;             // Hvor raskt kulen beveger seg

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Finn retningen fremover basert på transform
        rb.linearVelocity = transform.up * speed;

        // Ødelegg kulen etter X sekunder
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            float diff = 1f;
            if (DifficultyManager.Instance != null)
                diff = DifficultyManager.Instance.GetDifficultyValue();

            // Additiv vanskelighetslogikk
            float totalDamage = baseDamage + diff;

            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(Mathf.RoundToInt(totalDamage)); // Konverter float → int

            Destroy(gameObject);
        }
    }
}
