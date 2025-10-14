using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DifficultyStructure : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 2.5f;     // Hvor nært spilleren må være
    public float maxBonus = 3f;            // Maks hvor mye ekstra vanskelighet den kan gi
    public bool oneTimeUse = true;         // Skal den kunne brukes bare én gang?

    [Header("Visual Feedback (Optional)")]
    public SpriteRenderer highlightSprite; // F.eks. glød-effekt når spilleren er nær
    public Color activeColor = Color.yellow;
    public Color defaultColor = Color.white;

    private Transform player;
    private bool playerInRange = false;
    private bool used = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (highlightSprite != null)
            highlightSprite.color = defaultColor;
    }

    void Update()
    {
        if (player == null || used) return;

        float dist = Vector2.Distance(transform.position, player.position);
        playerInRange = dist <= interactRange;

        // Endre farge hvis spilleren er nær
        if (highlightSprite != null)
            highlightSprite.color = playerInRange ? activeColor : defaultColor;

        // Trykk E for å aktivere
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ActivateStructure();
        }
    }

    void ActivateStructure()
    {
        if (DifficultyManager.Instance == null)
        {
            Debug.LogWarning("Ingen DifficultyManager i scenen!");
            return;
        }

        float bonus = Random.Range(1f, maxBonus);
        DifficultyManager.Instance.ModifyBaseDifficulty(bonus);

        Debug.Log($"🪨 Structure activated! Increased base difficulty by {bonus:F1}");

        if (oneTimeUse)
        {
            used = true;
            if (highlightSprite != null)
                highlightSprite.color = Color.gray;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
