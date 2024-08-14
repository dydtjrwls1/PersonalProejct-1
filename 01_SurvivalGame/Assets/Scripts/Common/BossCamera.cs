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
        // 첫 위치는 플레이어위치로 간다.
        Vector3 playerPosition = GameManager.Instance.Player.transform.position;
        transform.position =  playerPosition + Vector3.back * 10.0f;

        boss = FindObjectOfType<Boss>(true).transform;
        // 초당 움직일 거리를 구한다 (플레이어와 보스 사이의 거리 / 5 => 보스 등장 애니메이션이 5초이기 때문에)
        distancePerSecond = Vector3.Distance(playerPosition, boss.position) * 0.2f;

        // 보스용 카메라가 움직일 방향
        direction = (boss.position - playerPosition).normalized;
    }

    private void LateUpdate()
    {
        transform.Translate(Time.deltaTime * distancePerSecond * direction);
    }
}
