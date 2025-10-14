using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int gold = 0;
    public int level = 1;
    public int score = 0;
    public float currentXP = 0;
    public float xpToNextLevel = 100f;

    [Header("UI References")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public Slider xpSlider;

    private void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
            LevelUp();

        UpdateUI();
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        level++;
        xpToNextLevel *= 1.25f;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (goldText) goldText.text = "Gold: " + gold;
        if (scoreText) scoreText.text = "Score: " + score;
        if (levelText) levelText.text = "Lv " + level;

        if (xpSlider)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXP;
        }
    }
}
