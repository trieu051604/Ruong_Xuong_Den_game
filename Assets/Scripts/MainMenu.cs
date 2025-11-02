using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    // Called when Options button is clicked
    public void OpenOptions()
    {
        Debug.Log("Options menu opened");
        // Later, you can show a panel or popup for sound, graphics, etc.
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
