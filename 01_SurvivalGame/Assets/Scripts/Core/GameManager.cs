using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    PlayerBase player;

    ScoreText scoreText;

    public PlayerBase Player
    {
        get
        {
            if (player == null)
                player = FindAnyObjectByType<PlayerBase>();
            return player;
        }
    }

    public ScoreText ScoreText
    {
        get
        {
            if (scoreText == null)
                scoreText = FindAnyObjectByType<ScoreText>();
            return scoreText;
        }
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<PlayerBase>();

        scoreText = FindAnyObjectByType<ScoreText>();
    }
}
