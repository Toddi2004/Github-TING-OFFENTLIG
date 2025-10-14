using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;                // hvor raskt fienden beveger seg
    public int baseDamage = 10;             // grunnskade
    public float damageIncreasePerSecond = 0.5f; // hvor mye skaden øker per sekund

    private Transform player;               // referanse til spilleren
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // ingen fysikkpåvirkning
        rb.freezeRotation = true;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        // beregn ny posisjon mot spilleren
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPos = (Vector2)transform.position + direction * speed * Time.deltaTime;
        rb.MovePosition(newPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // regn ut total skade basert på hvor lenge spillet har kjørt
            int totalDamage = Mathf.RoundToInt(baseDamage + Time.timeSinceLevelLoad * damageIncreasePerSecond);

            // finn PlayerHealth-komponenten og gi skade
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(totalDamage);
            }

            // valgfritt: dytt fienden litt bort for "feedback"
            Vector2 knockback = (transform.position - player.position).normalized * 0.2f;
            transform.position += (Vector3)knockback;
        }
    }
}
