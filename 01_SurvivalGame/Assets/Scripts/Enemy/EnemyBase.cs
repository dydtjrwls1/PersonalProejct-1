using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyBase : RecycleObject
{
    [Header("적 기본 정보")]
    // 기본 이동속도
    public float speed = 5.0f;

    // 총 체력
    public int maxHP = 2;

    // 죽었을 때 오르는 점수
    public int point = 10;

    // 죽었을 때 나오는 경험치 양
    public int exp = 1;

    protected Animator animator;

    protected Rigidbody2D rb;

    // 살아있으면 true
    protected bool isAlive = true;

    // 플레이어 트랜스폼
    static protected Transform player;

    // 애니메이션 파라미터용 해시값
    readonly int HitParameter_Hash = Animator.StringToHash("Hit");
    readonly int DeadParameter_Hash = Animator.StringToHash("Dead");

    ScoreText scoreText;

    SpriteRenderer sr;

    // 현재 나아가는 방향
    Vector3 m_Direction;

    public Vector3 Direction
    {
        get => m_Direction;
        set
        {
            m_Direction = value;
        }
    }
    // 현재 체력
    int hp;

    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            // 체력이 0 이하면 Die 실행 그 외엔 Hit 피격 애니메이션 실행
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
    /// 적의 물리적인 움직임
    /// </summary>
    protected virtual void FixedUpdate()
    {
        rb.MovePosition(transform.position + Time.deltaTime * speed * Direction);
        Move();
    }

    // 모든 적의 기본적인 움직임 (플레이어를 추적한다)
    protected virtual void Update()
    {
        Vector3 playerPosition = player.position;
        Direction = (playerPosition - transform.position).normalized;
        sr.flipX = isAlive ? (transform.position.x > playerPosition.x) : sr.flipX; // 적이 플레이어 보다 오른쪽에 있으면 좌우반전 (살아 있을 때만)
        
    }

    /// <summary>
    /// 피격 시 체력이 깎이고 충돌 오브젝트 비활성화
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
