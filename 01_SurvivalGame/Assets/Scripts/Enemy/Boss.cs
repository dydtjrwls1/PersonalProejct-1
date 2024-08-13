using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(StartAction());
    }

    IEnumerator StartAction()
    {

        float orgSpeed = speed;
        speed = 0.0f;

        yield return new WaitForSeconds(5.0f);

        speed = orgSpeed;
    }
}
