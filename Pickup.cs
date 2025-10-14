using UnityEngine;

public enum PickupType { Gold, XP }

public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public PickupType pickupType;
    public int value = 5;
    public float moveSpeed = 3f;
    public float attractRange = 3f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < attractRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        PlayerStats stats = col.GetComponent<PlayerStats>();
        if (stats == null) return;

        switch (pickupType)
        {
            case PickupType.Gold:
                stats.AddGold(value);
                break;
            case PickupType.XP:
                stats.AddXP(value);
                break;
        }

        Debug.Log($"[Pickup] {pickupType} collected (+{value})");
        Destroy(gameObject);
    }
}
