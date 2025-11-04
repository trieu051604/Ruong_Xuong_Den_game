using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        currentEnergy = 0;
        boss.SetActive(false);
        MainMenu();

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
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
    public void MainMenu()
    {
        mainMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void PauseGameMenu()
    {
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
