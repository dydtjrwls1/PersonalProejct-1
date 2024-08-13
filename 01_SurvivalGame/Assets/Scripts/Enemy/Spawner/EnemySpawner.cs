using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public enum enemyType
    {
        Zombie = 1,
        StrongZombie = 2,
        Skeleton = 4
    }

    [Serializable]
    public struct spawnData
    {
        public float spawnInterval;
        public enemyType type; 
    }

    public spawnData[] spawnDatas;

    // 이번에 Spawn 할 spawnPoint 의 index
    int pointIndex;

    int bossWave = 8;

    // 이번에 Spawn 될 위치
    Vector3 spawnPosition;

    // spawnPoint 들을 담은 배열
    SpawnPoint[] spawnPoints;

    // player 위치 정보를 받기 위한 변수
    PlayerBase player;

    // player 위치정보를 담는 변수
    Vector3 playerPosition;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        player = FindAnyObjectByType<PlayerBase>();
    }

    private void Start()
    {
        foreach (var data in spawnDatas)
        {
            StartCoroutine(SpawnStart(data));
        }
    }


    /// <summary>
    /// 매 Spawn 마다 랜덤한 spawnPoint 들 중 한곳에서 랜덤한 위치 반환 후 플레이어 위치에서 더한만큼에서 spawn 한다.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnStart(spawnData data)
    {
        while (true)
        {
            int currentWave = GameManager.Instance.Wave;

            // currentWave 가 8 일경우 모든 쫄몹 소환을 정지시키고 보스 소환
            if(currentWave == bossWave)
            {
                Boss boss = FindObjectOfType<Boss>(true);
                boss.gameObject.SetActive(true);
                boss.transform.position = player.transform.position + new Vector3(7, 0, 0);
                StopAllCoroutines();
                break;
            }

            if (currentWave == ((int)data.type | currentWave)) 
            {
                yield return new WaitForSeconds(Mathf.Max(data.spawnInterval, 1.0f));
                Vector3 spawnPos = GetSpawnPosition();

                switch (data.type)
                {
                    case enemyType.Zombie:
                        Factory.Instance.GetZombie(spawnPos);
                        break;
                    case enemyType.StrongZombie:
                        Factory.Instance.GetStrongZombie(spawnPos);
                        break;
                    case enemyType.Skeleton:
                        Factory.Instance.GetSkeleton(spawnPos);
                        break;
                }
            }
            else { yield return null; } 
        }
    }
    Vector3 GetSpawnPosition()
    {
        playerPosition = player.transform.position; // 현재 player의 위치
        pointIndex = Random.Range(0, spawnPoints.Length);   // spawn 할 spawnPoint 정하기
        spawnPosition = spawnPoints[pointIndex].GetSpawnPoint();    // spawnPoint 에서 랜덤한 위치 받기
        return playerPosition + spawnPosition;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.green;
        if (Application.isPlaying)
        {
            foreach (var point in spawnPoints)
            {
                Vector3 leftPoint = playerPosition + new Vector3(point.minX, point.minY);
                Vector3 rightPoint = playerPosition + new Vector3(point.maxX, point.maxY);
                Gizmos.DrawLine(leftPoint, rightPoint);
            }
        }
        
    }
#endif
}