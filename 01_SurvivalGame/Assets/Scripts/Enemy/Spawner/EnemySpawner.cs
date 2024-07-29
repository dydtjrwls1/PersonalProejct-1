using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Spawn ����
    public float spawnInterval = 1.0f;

    // �̹��� Spawn �� spawnPoint �� index
    int pointIndex;

    // �̹��� Spawn �� ��ġ
    Vector3 spawnPosition;

    // spawnPoint ���� ���� �迭
    SpawnPoint[] spawnPoints;

    // player ��ġ ������ �ޱ� ���� ����
    PlayerBase player;

    // player ��ġ������ ��� ����
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
    /// �� Spawn ���� ������ spawnPoint �� �� �Ѱ����� ������ ��ġ ��ȯ �� �÷��̾� ��ġ���� ���Ѹ�ŭ���� spawn �Ѵ�.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            playerPosition = player.transform.position; // ���� player�� ��ġ
            pointIndex = Random.Range(0, spawnPoints.Length);   // spawn �� spawnPoint ���ϱ�
            spawnPosition = spawnPoints[pointIndex].GetSpawnPoint();    // spawnPoint ���� ������ ��ġ �ޱ�
            Factory.Instance.GetZombie(playerPosition + spawnPosition); // spawnPoint ���� ���� ��ġ�� ���� �÷��̾� ��ġ�� ���� ��ġ�� Spawn �Ѵ�
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

