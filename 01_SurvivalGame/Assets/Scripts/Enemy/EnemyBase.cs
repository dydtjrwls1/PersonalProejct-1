using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("적 기본 정보")]
    public float speed = 5.0f;

    public int maxHP = 2;

    int hp = 2;
    public int HP
    {
        get { return hp; } 
        set { hp = value; }
    }

    PlayerBase player;

    SpriteRenderer sr;

    protected virtual void Awake()
    {
        player = FindAnyObjectByType<PlayerBase>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hp = maxHP;
    }

    // 모든 적의 기본적인 움직임 (플레이어를 추적한다)
    protected virtual void Update()
    {
        Vector3 playerPosition = player.transform.position;
        transform.Translate(Time.deltaTime * speed * (playerPosition - transform.position).normalized);

        // 적이 플레이어 보다 오른쪽에 있으면 좌우반전
        sr.flipX = (transform.position.x > playerPosition.x);
    }
}
