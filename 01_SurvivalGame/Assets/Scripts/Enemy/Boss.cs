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

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.Player.onDie?.Invoke();
    }

    IEnumerator StartAction()
    {
        float orgSpeed = speed;
        speed = 0.0f;
        animator.SetTrigger("Appear");
        rb.simulated = false;
        transform.position = player.position + Vector3.right * 15.0f;

        yield return new WaitForSeconds(5.0f);

        speed = orgSpeed;
        rb.simulated = true;
    }
}
