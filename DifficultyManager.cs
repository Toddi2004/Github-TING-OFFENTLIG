using UnityEngine;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Difficulty Settings")]
    public float timeScaleRate = 0.1f;   // Hvor mye vanskeligheten øker per sekund
    public float pickupIncrease = 0.5f;  // Hvor mye pickups øker vanskelighet
    public float baseMultiplier = 1f;    // Grunnverdi (kan endres av structures)

    private float difficulty = 1f;       // Intern progresjon over tid

    [Header("UI")]
    public TMP_Text difficultyText;      // TextMeshPro UI-element

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        // Øk vanskelighetsnivået gradvis over tid
        difficulty += Time.deltaTime * timeScaleRate;

        // Oppdater UI
        if (difficultyText != null)
            difficultyText.text = $"Difficulty: {GetDifficultyValue():F2}";
    }

    // Returnerer nåværende vanskelighetsverdi
    public float GetDifficultyValue()
    {
        return difficulty + baseMultiplier;
    }

    // Øker vanskelighet ved pickup (XP/gull osv.)
    public void IncreaseDifficultyFromPickup()
    {
        difficulty += pickupIncrease;
    }

    // Øker vanskelighet direkte (brukes av fiender)
    public void AddDifficulty(float amount)
    {
        difficulty += amount;
    }

    // Justerer base multiplier (f.eks. via structures)
    public void ModifyBaseDifficulty(float amount)
    {
        baseMultiplier += amount;
        Debug.Log($"[DifficultyManager] Base difficulty increased by {amount:F1}. New baseMultiplier = {baseMultiplier:F2}");
    }
}
