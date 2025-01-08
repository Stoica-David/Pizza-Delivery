using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
