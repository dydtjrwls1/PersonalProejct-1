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

    // player 위치 정보를 받기 위한 변수
    PlayerBase player;

    // player 위치정보를 담는 변수
    Vector3? playerPosition = null;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        player = FindAnyObjectByType<PlayerBase>();
    }

    private void Start()
    {
        StartCoroutine(Spawn());
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
            playerPosition = player.transform.position; // 현재 player의 위치
            pointIndex = Random.Range(0, spawnPoints.Length);   // spawn 할 spawnPoint 정하기
            spawnPosition = spawnPoints[pointIndex].GetSpawnPoint();    // spawnPoint 에서 랜덤한 위치 받기
            Factory.Instance.GetZombie(playerPosition + spawnPosition); // spawnPoint 에서 받은 위치와 현재 플레이어 위치를 더한 위치에 Spawn 한다
        }
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (playerPosition != null)
        {
            foreach (var point in spawnPoints)
            {
                Vector3 leftPoint = playerPosition.GetValueOrDefault() + new Vector3(point.minX, point.minY);
                Vector3 rightPoint = playerPosition.GetValueOrDefault() + new Vector3(point.maxX, point.maxY);
                Gizmos.DrawLine(leftPoint, rightPoint);
            }
        }
        
    }
#endif
}

