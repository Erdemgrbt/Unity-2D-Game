using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameoverScore : MonoBehaviour
{
    [SerializeField] TMP_Text scoreDisplayText;
    [SerializeField] TMP_Text highScoreDisplayText;

    private void Start()
    {
        DisplayScore();
        DisplayHighScore();
    }

    private void DisplayScore()
    {
        scoreDisplayText.text = "Your Score : " + Score.score;
    }

    private void DisplayHighScore()
    {
        highScoreDisplayText.text = "High Score : " + PlayerPrefs.GetInt("highScore").ToString();
    }
}
