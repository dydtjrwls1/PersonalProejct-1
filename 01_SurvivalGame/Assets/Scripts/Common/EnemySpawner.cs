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

    // player ��ġ ������ �ǽð����� �ޱ� ���� ����
    PlayerBase player;

    // player ��ġ�� �ǽð� ������ ��� ����
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
        // player�� �ǽð� ��ġ ���� ����
        playerPosition = player.transform.position;
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
            pointIndex = Random.Range(0, spawnPoints.Length);   // spawn �� spawnPoint ���ϱ�
            spawnPosition = spawnPoints[pointIndex].GetSpawnPoint();    // spawnPoint ���� ������ ��ġ �ޱ�
            Factory.Instance.GetZombie(playerPosition + spawnPosition); // spawnPoint ���� ���� ��ġ�� ���� �÷��̾� ��ġ�� ���� ��ġ�� Spawn �Ѵ�
        }
        
    }
}
