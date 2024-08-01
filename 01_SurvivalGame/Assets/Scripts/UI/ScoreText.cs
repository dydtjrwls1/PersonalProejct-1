using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    // ���� �������� ����
    float currentScore = 0;

    // �����ؾ��� ����
    int targetScore;

    int score = 0;

    // ���� ������ �ӵ�
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
        // ���� ������ �����ؾ��� ������ �ɶ�����
        if (Score < targetScore)
        {
            currentScore += (targetScore - Score) * Time.deltaTime * raiseSpeed;
            currentScore = Mathf.Min(currentScore, targetScore); // Ÿ�� �������� ������ ��츦 �����Ѵ�.
            Score = Mathf.RoundToInt(currentScore);
        }
    }

    public void AddScore(int point)
    {
        targetScore += point;
    }
}
