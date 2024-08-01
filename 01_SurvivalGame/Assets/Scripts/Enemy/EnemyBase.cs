using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyBase : RecycleObject
{
    [Header("�� �⺻ ����")]
    public float speed = 5.0f;

    public int maxHP = 2;

    public int point = 10;

    static ScoreText scoreText;

    protected Animator animator;

    protected Rigidbody2D rb;

    protected bool isAlive = true;

    readonly int HitParameter_Hash = Animator.StringToHash("Hit");

    readonly int DeadParameter_Hash = Animator.StringToHash("Dead");

    protected Transform player;

    SpriteRenderer sr;

    Vector3 direction;

    int hp;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;

            // ü���� 0 ���ϸ� Die ���� �� �ܿ� Hit �ǰ� �ִϸ��̼� ����
            if (hp < 1)
            {
                Die();
            } else
            {
                animator.SetTrigger(HitParameter_Hash);
            }


        }
    }

    protected virtual void Awake()
    {
        player = GameManager.Instance.Player.transform;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        scoreText = FindAnyObjectByType<ScoreText>();
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isAlive = true;
        hp = maxHP;
    }

    /// <summary>
    /// ���� �������� ������
    /// </summary>
    protected virtual void FixedUpdate()
    {
        rb.MovePosition(transform.position + Time.deltaTime * speed * direction);
        Move();
    }

    // ��� ���� �⺻���� ������ (�÷��̾ �����Ѵ�)
    private void Update()
    {
        Vector3 playerPosition = player.position;
        direction = (playerPosition - transform.position).normalized;
        sr.flipX = isAlive ? (transform.position.x > playerPosition.x) : sr.flipX; // ���� �÷��̾� ���� �����ʿ� ������ �¿���� (��� ���� ����)
        
    }

    /// <summary>
    /// �ǰ� �� ü���� ���̰� �浹 ������Ʈ ��Ȱ��ȭ
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HP -= 1;
            collision.gameObject.SetActive(false);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   
    protected virtual void Move()
    {

    }

    
    protected virtual void Die()
    {
        isAlive = false;
        rb.simulated = false;
        scoreText.AddScore(point);
        animator.SetTrigger(DeadParameter_Hash);
        Factory.Instance.GetCoin(transform.position);
        DIsableTimer(1.0f);
    }
}
