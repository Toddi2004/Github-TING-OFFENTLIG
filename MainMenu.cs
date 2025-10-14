using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI highScoreText; // Dra inn HighScoreText fra Canvas i Inspector

    void Start()
    {
        // Hent lagret highscore fra PlayerPrefs (0 hvis det ikke finnes)
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    // Kalles når man trykker "Start Game"-knappen
    public void StartGame()
    {
        // Laster spillscenen (bruk nøyaktig navn på scenen din)
        SceneManager.LoadScene("MainScene");
    }

    // (valgfritt) legg til en Exit-knapp senere
    public void QuitGame()
    {
        Debug.Log("Quit Game pressed");
        Application.Quit();
    }
}
