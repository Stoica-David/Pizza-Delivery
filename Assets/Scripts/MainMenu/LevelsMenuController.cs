using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenuController : MonoBehaviour
{

    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

