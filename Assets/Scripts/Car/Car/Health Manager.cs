using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static int health;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;


        void Start()
        {
            if (SceneManager.GetActiveScene().name == "Level 1")
            {
                health = 3;
            }
            else if (SceneManager.GetActiveScene().name == "Level 2")
            {
                health = 2;
            }
            else if (SceneManager.GetActiveScene().name == "Level 3")
            {
                health = 1;
            }
            else if (SceneManager.GetActiveScene().name == "Infinite Runner")
            {
                health = 3;
            }

        }
    void Update()
    {
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }
        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }
}