using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    // 현재 보여지는 점수
    float currentScore = 0;

    // 도달해야할 점수
    int targetScore;

    int score = 0;

    // 점수 오르는 속도
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
        // 실제 점수가 도달해야할 점수가 될때까지
        if (Score < targetScore)
        {
            currentScore += (targetScore - Score) * Time.deltaTime * raiseSpeed;
            currentScore = Mathf.Min(currentScore, targetScore); // 타겟 점수보다 높아질 경우를 방지한다.
            Score = Mathf.RoundToInt(currentScore);
        }
    }

    public void AddScore(int point)
    {
        targetScore += point;
    }
}
