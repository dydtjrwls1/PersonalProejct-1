using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongZombie : EnemyBase
{
    int barrageCount = 4;

    protected override void Die()
    {
        base.Die();

        // StrongZombie 사망 시 사방으로 총알을 흩뿌린다
        SpawnBullet();
    }

    /// <summary>
    /// 사망 시 사방으로 총알을 흩뿌리는 함수
    /// </summary>
    void SpawnBullet()
    {
        for (int i = 0; i < barrageCount; i++)
        {
            // factory 에서 bullet 가져오기
            EnemyBullet bullet = Factory.Instance.GetEnemyBullet(transform.position);

            // 현재 위치에서 회전된 up 벡터를 더해서 목적지 정하기
            Quaternion rotateAngle = Quaternion.AngleAxis(45 + 90 * i, Vector3.forward);
            Vector3 destination = transform.position + rotateAngle * Vector3.up;

            bullet.setDestination(destination);
        }
    }
}
