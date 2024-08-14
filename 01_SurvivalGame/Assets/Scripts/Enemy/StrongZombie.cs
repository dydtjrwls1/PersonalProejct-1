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
            EnemyBullet bullet = Factory.Instance.GetEnemyBullet(transform.position);
            bullet.setDestination(transform.up + Vector3.right * i);
        }
    }
}
