using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentEnergy;
    [SerializeField] private int energyThreshold = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject spawner;
    private bool bossCalled = false;
    [SerializeField] private Image energryBar;
    [SerializeField] private GameObject gameUi;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;

    public int ammoForThisLevel = 30;

    void Start()
    {
        currentEnergy = 0;
        if (boss != null) boss.SetActive(false);
        MainMenu();
        Time.timeScale = 1f;
    }

    public void AddEnergy()
    {
        if (bossCalled) return;
        currentEnergy += 1;
        UpdateEnergyBar();
        if (currentEnergy >= energyThreshold)
        {
            CallBoss();
        }
    }

    public void CallBoss()
    {
        bossCalled = true;
        if (boss != null) boss.SetActive(true);
        if (spawner != null) spawner.SetActive(false);
        if (gameUi != null) gameUi.SetActive(false);
    }

    private void UpdateEnergyBar()
    {
        if (energryBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / (float)energyThreshold);
            energryBar.fillAmount = fillAmount;
        }
    }

    public void MainMenu()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    public void GameOverMenu()
    {
        if (gameOverMenu != null) gameOverMenu.SetActive(true);
        if (mainMenu != null) mainMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    public void PauseGameMenu()
    {
        if (pauseMenu != null) pauseMenu.SetActive(true);
        if (mainMenu != null) mainMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
