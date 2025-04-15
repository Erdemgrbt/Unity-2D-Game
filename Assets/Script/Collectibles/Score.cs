using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    public static int score;
    [SerializeField] private AudioClip pickupSound;

    private void Start()
    {
        score = 0;
        scoreText.text = "Score : " + score.ToString();
    }

    private void Update()
    {
        scoreText.text = "Score : " + score.ToString();

        if(score > PlayerPrefs.GetInt("highScore"))
            PlayerPrefs.SetInt("highScore", score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Gem")
        {
            Destroy(other.gameObject);
            SoundManager.instance.PlaySound(pickupSound);
            score++;
        }
    }
}
