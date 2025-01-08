using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI;

    public static bool isOver = false;

    private void Start()
    {
        gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (isOver)
        {
            Pause();
        }
    }


    public void Restart()
    {
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        isOver = false;
        AudioListener.pause = false;
        Physics.IgnoreLayerCollision(7, 8, false);
        Physics.IgnoreLayerCollision(7, 9, false);
        gameOverUI.SetActive(false);

    }

    public void Pause()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;

        
        gameOverUI.SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
