using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    PlayerBase player;

    ScoreText scoreText;

    int wave = 1;

    public Action<int> onWaveChange = null;

    public int Wave
    {
        get => wave;
        set
        {
            wave = value;
            wave = Mathf.Clamp(wave, 1, 8);
            onWaveChange?.Invoke(wave);
            if (wave == 8)
                SpawnBoss();
                StartCoroutine(CameraMoving());
        }
    }

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

    void SpawnBoss()
    {
        Boss boss = FindObjectOfType<Boss>(true);
        boss.gameObject.SetActive(true);
    }

    IEnumerator CameraMoving()
    {
        Camera[] cameras = FindObjectsOfType<Camera>(true);

        cameras[0].enabled = !cameras[0].enabled;
        cameras[1].gameObject.SetActive(true);

        yield return new WaitForSeconds(5.0f);

        cameras[0].enabled = !cameras[0].enabled;
        cameras[1].gameObject.SetActive(false);
    }
}
