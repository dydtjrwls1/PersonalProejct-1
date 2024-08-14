using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : RecycleObject
{
    public float disableTime = 5.0f;

    public float moveSpeed = 3.0f;

    Vector3 direction;

    protected override void OnEnable()
    {
        base.OnEnable();
        DIsableTimer(disableTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * moveSpeed * direction);
    }

    public void setDestination(Vector3 destination)
    {
        direction = (destination - transform.position).normalized;
    }
}
