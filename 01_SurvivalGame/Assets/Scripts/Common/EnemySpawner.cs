using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Spawn 간격
    public float spawnInterval = 1.0f;

    // 이번에 Spawn 할 spawnPoint 의 index
    int pointIndex;

    // 이번에 Spawn 될 위치
    Vector3 spawnPosition;

    // spawnPoint 들을 담은 배열
    SpawnPoint[] spawnPoints;

    // player 위치 정보를 실시간으로 받기 위한 변수
    PlayerBase player;

    // player 위치의 실시간 정보를 담는 변수
    Vector3 playerPosition;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        player = FindAnyObjectByType<PlayerBase>();
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        // player의 실시간 위치 정보 갱신
        playerPosition = player.transform.position;
    }

    /// <summary>
    /// 매 Spawn 마다 랜덤한 spawnPoint 들 중 한곳에서 랜덤한 위치 반환 후 플레이어 위치에서 더한만큼에서 spawn 한다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            pointIndex = Random.Range(0, spawnPoints.Length);   // spawn 할 spawnPoint 정하기
            spawnPosition = spawnPoints[pointIndex].GetSpawnPoint();    // spawnPoint 에서 랜덤한 위치 받기
            Factory.Instance.GetZombie(playerPosition + spawnPosition); // spawnPoint 에서 받은 위치와 현재 플레이어 위치를 더한 위치에 Spawn 한다
        }
        
    }
}
