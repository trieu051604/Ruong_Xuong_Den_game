using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject mainMenuPanel;
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    // Called when Options button is clicked
    public void OpenInstruction()
    {
        Debug.Log("Instruction panel opened");

        if (instructionPanel != null)
            instructionPanel.SetActive(true);
    }
    public void BackToMainMenu()
    {
        Debug.Log("Returning to main menu");

        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }
    // Called when Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();

        // This line is only for testing in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
