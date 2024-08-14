using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongZombie : EnemyBase
{
    int barrageCount = 4;

    protected override void Die()
    {
        base.Die();

        // StrongZombie ��� �� ������� �Ѿ��� ��Ѹ���
        SpawnBullet();
    }

    /// <summary>
    /// ��� �� ������� �Ѿ��� ��Ѹ��� �Լ�
    /// </summary>
    void SpawnBullet()
    {
        for (int i = 0; i < barrageCount; i++)
        {
            // factory ���� bullet ��������
            EnemyBullet bullet = Factory.Instance.GetEnemyBullet(transform.position);

            // ���� ��ġ���� ȸ���� up ���͸� ���ؼ� ������ ���ϱ�
            Quaternion rotateAngle = Quaternion.AngleAxis(45 + 90 * i, Vector3.forward);
            Vector3 destination = transform.position + rotateAngle * Vector3.up;

            bullet.setDestination(destination);
        }
    }
}
