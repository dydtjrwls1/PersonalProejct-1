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

    // �̹��� Spawn �� spawnPoint �� index
    int pointIndex;

    int bossWave = 8;

    // �̹��� Spawn �� ��ġ
    Vector3 spawnPosition;

    // spawnPoint ���� ���� �迭
    SpawnPoint[] spawnPoints;

    // player ��ġ ������ �ޱ� ���� ����
    PlayerBase player;

    // player ��ġ������ ��� ����
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
    /// �� Spawn ���� ������ spawnPoint �� �� �Ѱ����� ������ ��ġ ��ȯ �� �÷��̾� ��ġ���� ���Ѹ�ŭ���� spawn �Ѵ�.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnStart(spawnData data)
    {
        while (true)
        {
            int currentWave = GameManager.Instance.Wave;

            // currentWave �� 8 �ϰ�� ��� �̸� ��ȯ�� ������Ű�� ���� ��ȯ
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
        playerPosition = player.transform.position; // ���� player�� ��ġ
        pointIndex = Random.Range(0, spawnPoints.Length);   // spawn �� spawnPoint ���ϱ�
        spawnPosition = spawnPoints[pointIndex].GetSpawnPoint();    // spawnPoint ���� ������ ��ġ �ޱ�
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