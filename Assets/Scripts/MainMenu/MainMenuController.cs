using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayInfiniteRunner()
    {
        SceneManager.LoadScene("Infinite Runner");
    }

    public void ShowLevels()
    {
        SceneManager.LoadScene("Levels"); 
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
