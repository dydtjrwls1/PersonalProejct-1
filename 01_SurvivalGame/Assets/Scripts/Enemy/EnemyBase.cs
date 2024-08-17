using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyBase : RecycleObject
{
    [Header("�� �⺻ ����")]
    // �⺻ �̵��ӵ�
    public float speed = 5.0f;

    // �� ü��
    public int maxHP = 2;

    // �׾��� �� ������ ����
    public int point = 10;

    // �׾��� �� ������ ����ġ ��
    public int exp = 1;

    protected Animator animator;

    protected Rigidbody2D rb;

    // ��������� true
    protected bool isAlive = true;

    // �÷��̾� Ʈ������
    static protected Transform player;

    // �ִϸ��̼� �Ķ���Ϳ� �ؽð�
    readonly int HitParameter_Hash = Animator.StringToHash("Hit");
    readonly int DeadParameter_Hash = Animator.StringToHash("Dead");

    ScoreText scoreText;

    SpriteRenderer sr;

    // ���� ���ư��� ����
    Vector3 m_Direction;

    public Vector3 Direction
    {
        get => m_Direction;
        set
        {
            m_Direction = value;
        }
    }
    // ���� ü��
    int hp;

    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            // ü���� 0 ���ϸ� Die ���� �� �ܿ� Hit �ǰ� �ִϸ��̼� ����
            if (hp < 1)
                Die();
            else
                animator.SetTrigger(HitParameter_Hash);
        }
    }

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        scoreText = GameManager.Instance.ScoreText;
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        player = GameManager.Instance.Player.transform;
        isAlive = true;
        hp = maxHP;
        rb.simulated = true;
    }

    /// <summary>
    /// ���� �������� ������
    /// </summary>
    protected virtual void FixedUpdate()
    {
        rb.MovePosition(transform.position + Time.deltaTime * speed * Direction);
        Move();
    }

    // ��� ���� �⺻���� ������ (�÷��̾ �����Ѵ�)
    protected virtual void Update()
    {
        Vector3 playerPosition = player.position;
        Direction = (playerPosition - transform.position).normalized;
        sr.flipX = isAlive ? (transform.position.x > playerPosition.x) : sr.flipX; // ���� �÷��̾� ���� �����ʿ� ������ �¿���� (��� ���� ����)
        
    }

    /// <summary>
    /// �ǰ� �� ü���� ���̰� �浹 ������Ʈ ��Ȱ��ȭ
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HP -= GameManager.Instance.Player.RangePower;
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Weapon"))
        {
            HP -= GameManager.Instance.Player.MeleePower;
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
        Coin coin = Factory.Instance.GetCoin(transform.position, exp);
        coin.ExpPoint = exp;
        DIsableTimer(1.0f);
    }

    void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
