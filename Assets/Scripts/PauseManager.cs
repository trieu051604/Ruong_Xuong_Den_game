using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    private bool isPaused = false;
    [SerializeField] private GameObject instructionPanel;
   
    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

}

void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void OpenInstruction()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(true);
    }
    public void CloseInstruction()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(false);
    }
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("main_menu");
    }
}
