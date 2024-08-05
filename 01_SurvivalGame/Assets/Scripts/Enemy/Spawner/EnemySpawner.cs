using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public enum enemyType
    {
        Zombie = 0,
        StrongZombie,
        Skeleton
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
            int currentWave = GameManager.Instance.wave;
            Vector3 spawnPos = GetSpawnPosition();
            yield return new WaitForSeconds(Mathf.Max(data.spawnInterval - currentWave, 1.0f));
            switch (data.type)
            {
                case enemyType.Zombie:
                    Factory.Instance.GetZombie(spawnPos); // spawnPoint ���� ���� ��ġ�� ���� �÷��̾� ��ġ�� ���� ��ġ�� Spawn �Ѵ�
                    break;
                case enemyType.StrongZombie:
                    Factory.Instance.GetStrongZombie(spawnPos); // spawnPoint ���� ���� ��ġ�� ���� �÷��̾� ��ġ�� ���� ��ġ�� Spawn �Ѵ�
                    break;
                case enemyType.Skeleton:
                    Factory.Instance.GetSkeleton(spawnPos);
                    break;
            }
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

