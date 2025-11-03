using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    public int currentEnergy;
    [SerializeField] private int energyThreshold = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject spawner;
    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject gameUi;

    [Header("Game Over UI")]
    public GameObject gameOverUI;

    private bool bossCalled = false;
    private bool isGameOver = false;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentEnergy = 0;
        boss.SetActive(true);

        if (gameOverUI != null)
            gameOverUI.SetActive(false); 
        else
            Debug.LogWarning("GameOverUI not assigned in GameManager!");

        if (gameUi != null)
            gameUi.SetActive(true); 
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
                Retry();

            if (Input.GetKeyDown(KeyCode.Escape))
                QuitGame();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls
        isGameOver = true;

        // Pause the game
        Time.timeScale = 0f;

        // Show Game Over UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Hide main game UI
        if (gameUi != null)
            gameUi.SetActive(false);
    }

    public void AddEnergy()
    {
        if (bossCalled) return;

        currentEnergy++;
        UpdateEnergyBar();

        if (currentEnergy >= energyThreshold)
            CallBoss();
    }

    public void CallBoss()
    {
        bossCalled = true;
        boss.SetActive(true);
        spawner.SetActive(false);

        if (gameUi != null)
            gameUi.SetActive(false); 
    }

    public void Retry()
    {
        Time.timeScale = 1f; // Reset time scale
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset time scale
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / energyThreshold);
            energyBar.fillAmount = fillAmount;
        }
    }
}
