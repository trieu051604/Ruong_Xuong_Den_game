using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    private int currentEnergy;
    [SerializeField] private int energyThreshold = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject spawner;
    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject gameUi;
    public int ammoForThisLevel = 30;

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverMenuUI;

    [Header("Audio")]
    [SerializeField] private AudioManagerMainMenu audioManagerMainMenu;
    [SerializeField] private AudioManagementLevel1 audioManagementLevel1;

    private bool bossCalled = false;
    private GameState currentState;

    void Start()
    {
        currentEnergy = 0;
        boss?.SetActive(false);
        SetGameState(GameState.MainMenu);
    }

    void Update()
    {
        // Optional: handle input like pause toggle
        if (Input.GetKeyDown(KeyCode.Escape) && currentState == GameState.Playing)
        {
            PauseGameMenu();
        }
    }

    public void AddEnergy()
    {
        if (bossCalled) return;

        currentEnergy++;
        UpdateEnergyBar();

        if (currentEnergy >= energyThreshold)
        {
            CallBoss();
        }
    }

    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / energyThreshold);
            energyBar.fillAmount = fillAmount;
        }
    }

    private void CallBoss()
    {
        bossCalled = true;
        boss?.SetActive(true);
        spawner?.SetActive(false);
        gameUi?.SetActive(false);
    }

    public void StartGame()
    {
        SetGameState(GameState.Playing);
        Time.timeScale = 1f;
        audioManagerMainMenu?.PlayBackgroundMusicMainMenu();
    }

    public void PauseGameMenu()
    {
        SetGameState(GameState.Paused);
        Time.timeScale = 0f;
        Debug.Log("Game Paused!");
    }

    public void ResumeGame()
    {
        SetGameState(GameState.Playing);
        Time.timeScale = 1f;
        Debug.Log("Game Resumed!");
    }

    public void GameOverMenu()
    {
        SetGameState(GameState.GameOver);
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }

    public void ReturnToMainMenu()
    {
        SetGameState(GameState.MainMenu);
        Time.timeScale = 0f;
    }

    private void SetGameState(GameState newState)
    {
        currentState = newState;

        mainMenuUI?.SetActive(newState == GameState.MainMenu);
        pauseMenuUI?.SetActive(newState == GameState.Paused);
        gameOverMenuUI?.SetActive(newState == GameState.GameOver);
        gameUi?.SetActive(newState == GameState.Playing);
    }
}
