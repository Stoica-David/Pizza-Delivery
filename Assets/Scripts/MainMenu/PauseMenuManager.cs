using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Păstrează meniul între scene
        }
        else
        {
            Destroy(gameObject); // Distruge instanțele duplicate
        }
    }
}