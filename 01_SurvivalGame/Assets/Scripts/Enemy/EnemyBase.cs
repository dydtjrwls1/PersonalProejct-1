using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("�� �⺻ ����")]
    public float speed = 5.0f;

    public int maxHP = 2;

    int hp;
    public int HP
    {
        get { return hp; } 
        set 
        {
            hp = value;
            if(hp < 1)
            {
                Die();
            }
        }
    }
    protected Rigidbody2D rb;

    PlayerBase player;

    SpriteRenderer sr;

    Vector3 direction;

    bool isAlive = true;

    protected virtual void Awake()
    {
        player = FindAnyObjectByType<PlayerBase>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isAlive = true;
        hp = maxHP;
    }
    private void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position + Time.deltaTime * speed * (Vector2)direction);
    }

    // ��� ���� �⺻���� ������ (�÷��̾ �����Ѵ�)
    protected virtual void Update()
    {
        Vector3 playerPosition = player.transform.position;
        direction = (playerPosition - transform.position).normalized;
        sr.flipX = isAlive ? (transform.position.x > player.transform.position.x) : sr.flipX; // ���� �÷��̾� ���� �����ʿ� ������ �¿���� (��� ���� ����)
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HP -= 1;
            collision.gameObject.SetActive(false);
        }
    }

    protected virtual void Die()
    {
        isAlive = false;
        rb.simulated = false;
        DIsableTimer(1.0f);
    }
}
