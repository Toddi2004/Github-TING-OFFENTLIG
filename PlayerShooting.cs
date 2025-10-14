using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint; // punktet der kula spawner

    [Header("Stats")]
    public float fireRate = 0.5f; // tid mellom skudd
    public float bulletSpeed = 10f;
    public float targetingRange = 10f; // hvor langt spilleren kan “se” etter fiender

    private float nextFireTime = 0f;

    void Update()
    {
        // Finn nærmeste fiende innenfor rekkevidde
        GameObject targetEnemy = FindNearestEnemy();

        // Hvis det finnes en fiende og nok tid har gått, skyt
        if (targetEnemy != null && Time.time >= nextFireTime)
        {
            Shoot(targetEnemy.transform);
            nextFireTime = Time.time + fireRate;
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float shortestDist = Mathf.Infinity;

        Vector3 playerPos = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(playerPos, enemy.transform.position);
            if (dist < shortestDist && dist <= targetingRange)
            {
                shortestDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void Shoot(Transform target)
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Beregn retning mot fienden
        Vector3 direction = (target.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Lag kula og sett retning
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        // Hvis kula har Rigidbody2D, gi den fart
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}
