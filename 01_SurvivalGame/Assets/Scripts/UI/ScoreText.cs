using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    float currentScore;

    int targetScore;

    int score = 0;

    float raiseSpeed = 50.0f;
     
    public int Score
    {

        get { return score; }
        private set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    
    }

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        Score = 0;
    }

    private void Update()
    {
        if (Score < targetScore)
        {
            currentScore += Time.deltaTime * raiseSpeed;
           
        }
    }

    public void AddScore(int point)
    {
        targetScore = Score + point;
        currentScore = Score;
    }
}
