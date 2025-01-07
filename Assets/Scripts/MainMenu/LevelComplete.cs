using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public bool levelWon = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if the int variable "currentPizzaBoxes" is equal to 0
        if (CarHandler.instance.currentPizzaBoxes == 0)
            //LevelWon();
            levelWon = true;
    }

    void LevelWon()
    {
        //close current scene and open the level won scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("LevelWon");
    }

    void LevelLost()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("LevelLost");
    }
}
