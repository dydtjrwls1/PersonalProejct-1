using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongZombie : EnemyBase
{
    int barrageCount = 4;

    protected override void Die()
    {
        base.Die();

        for(int i = 0; i < barrageCount; i++)
        {
            Quaternion rotateAngle = Quaternion.AngleAxis(45 + 90 * i, Vector3.forward);
            EnemyBullet bullet = Factory.Instance.GetEnemyBullet(transform.position);
            Vector3 destination = transform.position + rotateAngle * Vector3.up;
            bullet.setDestination(destination);
        }
    }
}
