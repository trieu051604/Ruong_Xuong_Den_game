using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [Header("UI References")]
    public GameObject GameOverPanel;
    public TextMeshProUGUI GameOverText;
    public Button RestartButton;
    public Button MainMenuButton;

    private void Awake()
    {
        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);

        if (RestartButton != null)
            RestartButton.onClick.AddListener(RestartGame);

        if (MainMenuButton != null)
            MainMenuButton.onClick.AddListener(MainMenu);

        if (GameOverText != null)
            GameOverText.text = "GAME OVER";

        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("main_menu");
    }
}
