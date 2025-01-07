using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI pizzasDeliveredText;

    public int pizzas = 0;
    int score = 0;
    int highScore = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = "SCORE: " + score.ToString();
        highScoreText.text = "HIGHSCORE: " + highScore.ToString();
        pizzasDeliveredText.text = "PIZZAS DELIVERED: " + pizzas.ToString();
    }

    void Update()
    {
        
    }

    public void AddPizza()
    {
        pizzas += 1;
        pizzasDeliveredText.text = "PIZZAS DELIVERED: " + pizzas.ToString();
    }
    public void ModifyPoints(int pointsToAdd = 1)
    {
        score += pointsToAdd;
        scoreText.text = "SCORE: " + score.ToString();

        if (highScore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }
}
