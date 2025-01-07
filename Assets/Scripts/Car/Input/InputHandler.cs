using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    CarHandler carHandler;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        carHandler.SetInput(input);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            carHandler.RestoreTimeCO();

            HealthManager.health = 3;

            Time.timeScale = 1;

            Physics.IgnoreLayerCollision(7, 8, false);
            Physics.IgnoreLayerCollision(7, 9, false);
        }
    }
}
