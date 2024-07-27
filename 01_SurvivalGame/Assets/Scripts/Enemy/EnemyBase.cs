using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("�� �⺻ ����")]
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

    // ��� ���� �⺻���� ������ (�÷��̾ �����Ѵ�)
    protected virtual void Update()
    {
        Vector3 playerPosition = player.transform.position;
        transform.Translate(Time.deltaTime * speed * (playerPosition - transform.position).normalized);

        // ���� �÷��̾� ���� �����ʿ� ������ �¿����
        sr.flipX = (transform.position.x > playerPosition.x);
    }
}
