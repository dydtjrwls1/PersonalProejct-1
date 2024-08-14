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

    const float CAMERA_SWITCH_TIME = 5.0f;

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
            {
                SpawnBoss();
                StartCoroutine(CameraMoving(CAMERA_SWITCH_TIME)); // 보스용 카메라 움직임으로 전환하는 코루틴 실행
            }
                
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

    // 메인 카메라와 보스용 카메라를 보스 등장 애니메이션 시간(5초) 만큼 전환한다.
    IEnumerator CameraMoving(float time)
    {
        Camera[] cameras = FindObjectsOfType<Camera>(true);

        cameras[0].enabled = !cameras[0].enabled;
        cameras[1].gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        cameras[0].enabled = !cameras[0].enabled;
        cameras[1].gameObject.SetActive(false);
    }
}
