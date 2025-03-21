using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;

    public static bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Physics.IgnoreLayerCollision(CarHandler.instance.layerToIgnore, CarHandler.instance.layerToIgnoreWith, false);
        Physics.IgnoreLayerCollision(CarHandler.instance.layerToIgnore, CarHandler.instance.otherLayerToIgnoreWith, false); 
        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
