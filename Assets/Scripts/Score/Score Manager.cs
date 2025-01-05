using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

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
    }

    void Update()
    {
        
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
