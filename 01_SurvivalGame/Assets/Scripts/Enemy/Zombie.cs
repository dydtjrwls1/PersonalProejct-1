using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : EnemyBase
{
    Animator animator;

    readonly int DeadParameter_Hash = Animator.StringToHash("Dead");
    readonly int HitParameter_Hash = Animator.StringToHash("Hit");

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool(DeadParameter_Hash, false);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        StartCoroutine(Hit());
    }

    protected override void Die()
    {
        base.Die();

        animator.SetTrigger(DeadParameter_Hash);
    }

    IEnumerator Hit()
    {
        animator.SetBool(HitParameter_Hash, true);

        yield return new WaitForSeconds(0.01f);

        animator.SetBool(HitParameter_Hash, false);
    }
}
