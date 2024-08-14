using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    Transform boss;

    float distancePerSecond;

    Vector3 direction;

    private void OnEnable()
    {
        // ù ��ġ�� �÷��̾���ġ�� ����.
        Vector3 playerPosition = GameManager.Instance.Player.transform.position;
        transform.position =  playerPosition + Vector3.back * 10.0f;

        boss = FindObjectOfType<Boss>(true).transform;
        // �ʴ� ������ �Ÿ��� ���Ѵ� (�÷��̾�� ���� ������ �Ÿ� / 5 => ���� ���� �ִϸ��̼��� 5���̱� ������)
        distancePerSecond = Vector3.Distance(playerPosition, boss.position) * 0.2f;

        // ������ ī�޶� ������ ����
        direction = (boss.position - playerPosition).normalized;
    }

    private void LateUpdate()
    {
        transform.Translate(Time.deltaTime * distancePerSecond * direction);
    }
}
