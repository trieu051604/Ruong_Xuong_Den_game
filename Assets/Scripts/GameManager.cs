using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentEnergy;
    [SerializeField] private int energyThreshold = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject spawner;
    private bool bossCalled = false;
    [SerializeField] private Image energryBar;
    [SerializeField] private GameObject gameUi;
    public int ammoForThisLevel = 30;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private AudioManagerMainMenu audioManagerMainMenu;
    //[SerializeField] private AudioManagementLevel1 audioManagementLevel1;
    void Start()
    {
        currentEnergy = 0;
        boss.SetActive(false);

        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (gameOverMenuUI != null) gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
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
        boss.SetActive(true);
        spawner.SetActive(false);
        gameUi.SetActive(false);
    }

    private void UpdateEnergyBar()
    {
        if (energryBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / (float)energyThreshold);
            energryBar.fillAmount = fillAmount;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        audioManagerMainMenu.PlayBackgroundMusicMainMenu();
    }

    public void PauseGameMenu()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Paused!");
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Game Resumed!");
    }

    public void GameOverMenu()
    {
        if (gameOverMenuUI != null) gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }
}
