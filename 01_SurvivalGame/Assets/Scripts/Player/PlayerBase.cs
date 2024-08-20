using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // 플레이어 기본 속도
    public float speed = 5.0f;

    // 총알이 발사되는 위치
    public Transform firePoint;

    // 무기 스프라이트
    public SpriteRenderer srWeapon;

    // 공격 간격
    public float fireInterval = 0.5f;

    // 대쉬 쿨타임
    public float dashCoolDown = 5.0f;

    // 액션 이벤트 델리게이트 함수
    public Action<int> levelUpAction = null;

    // 경험치 상승 이벤트 델리게이트 함수
    public Action<int, int> expUpAction = null;

    public Action<int> lifeChange = null;

    public Action onHeal = null;

    public Action onDie = null;

    public Action onHit = null;

    float currentDashCoolDown = 0.0f;

    // 플레이어 현재 속도
    float currentSpeed = 0.0f;

    bool onDash = true;

    // 플레이어 스프라이트의 좌우반전 확인용
    bool isFlipped = false;

    // 플레이어 사정거리 내에 적이 있는지 여부
    bool enemyInRange = false;

    // 무적 레이어 키 값
    int ImmuneLayerNum;

    // 플레이어 레이어 키 값
    int PlayerLayerNum;

    // 근접 무기 개수
    int meleeCount = 1;

    // 기본속도에 더해지는 속도
    float addedSpeed = 0.0f;

    // 한번에 발사할 총알의 개수
    int barrageCount = 1;

    // 총알 연속 발사 간격
    float barrageInterval = 0.1f;

    // 현재경험치
    int exp = 0;

    // 총 경험치
    int maxExp = 5;

    // 최대 체력
    int maxLife = 3;

    // 레벨
    int level = 1;

    // 공격력
    int meleePower = 1;

    int rangePower = 1;

    // 체력
    int life = 3;

    bool IsAlive => life > 0;

    PlayerInputAction action;

    Collider2D coll;

    // 플레이어 애니메이션 컨트롤러
    Animator animator;

    // 플레이어 스프라이트
    SpriteRenderer sr;

    // 대쉬 이펙트 스프라이트
    SpriteRenderer dashEffectSr;

    Rigidbody2D rb;

    // 가장 가까운 적의 Transform
    Transform nearestEnemy;

    // 사정거리 내의 적들의 Transform 배열
    List<Transform> enemiesInShootingZone = new List<Transform>();

    // 근접무기 배열
    Transform[] meleeWeapons = new Transform[5];

    // 대쉬 이펙트 트랜스폼
    Transform dashEffectTransform;

    // 플레이어 진행 방향
    Vector2 direction;

    // 스프라이트 원본 색
    Color originColor;

    // 피격시 나타날 스프라이트 색
    Color hitColor;

    public int Life
    {
        get => life;
        set
        {
            if (life != value)
            {
                if (life < value)
                {
                    life = value;
                    onHeal?.Invoke();

                    if (value > maxLife)
                        life = Mathf.Clamp(life, 0, maxLife);
                    
                }
                else
                {
                    life = value;

                    if (IsAlive)
                    {
                        onHit?.Invoke();
                        OnHit();
                    }
                    else
                        OnDie();
                }
                lifeChange?.Invoke(life);
            }
        }
    }

    // 속도 파라미터
    public float Speed
    {
        get { return currentSpeed; }
        set
        {
            currentSpeed = value;
            
            animator.SetFloat(SpeedParameter_Hash, currentSpeed);
        }
    }

    // 경험치 프로퍼티
    public int Exp
    {
        get => exp;
        set
        {
            exp = value;
            // 경험치가 max 경험치 초과 시 레벨업
            if (exp > maxExp - 1)
            {
                exp = exp - maxExp; // 남은 경험치 계산
                maxExp = Mathf.RoundToInt(maxExp * 1.4f);
                Level++;
                levelUpAction(Level); // Level UI 에 레벨업 이벤트 발생 알림
            }

            expUpAction(exp, maxExp); // Exp UI 에 경험치 상승 이벤트 발생 알림
        }
    }

    // 레벨 프로퍼티
    public int Level
    {
        get => level;
        set
        {
            level = value;
        }
    }

    // 공격력 프로퍼티
    public int MeleePower
    {
        get => meleePower;
        set
        {
            meleePower = value;
        }
    }

    public int RangePower
    {
        get => rangePower;
        set
        {
            rangePower = value;
        }
    }

    public float AddedSpeed
    {
        get => addedSpeed;
        set
        {
            addedSpeed = value;
        }
    }

    public int BarrageCount
    {
        get => barrageCount;
        set
        {
            barrageCount = value;
            barrageCount = Mathf.Clamp(barrageCount, 0, 5);
        }
    }

    public int MeleeCount
    {
        get => meleeCount;
        set
        {
            meleeCount = value;
            meleeCount = Mathf.Clamp(meleeCount, 1, 5);

            SetMeleeWeapon();
        }
    }

    public float CurrentDashCoolDown
    {
        get => currentDashCoolDown;
        set
        {
            currentDashCoolDown = Mathf.Max(0.0f, value); // 0 보다 밑은 존재할 수 없다.
        }
    }

    // 애니메이션 제어를 위한 Speed 파라미터의 해쉬번호
    readonly int SpeedParameter_Hash = Animator.StringToHash("Speed");

    readonly int DashParameter_Hash = Animator.StringToHash("Dash");

    // Start is called before the first frame update
    void Awake()
    {
        // 필요한 컴포넌트 초기화
        action = new PlayerInputAction();
        animator = GetComponent<Animator>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        ShootingZone shootingZone = GetComponentInChildren<ShootingZone>();
        shootingZone.onEnemyEnter += (enemy) => enemiesInShootingZone.Add(enemy);
        shootingZone.onEnemyExit += (enemy) => enemiesInShootingZone.Remove(enemy);

        dashEffectTransform = transform.GetChild(2);
        dashEffectSr = dashEffectTransform.GetComponent<SpriteRenderer>();


        // 무기 배열 초기화
        Transform spinPoint = transform.GetComponentInChildren<SpinPoint>().transform;
        for (int i = 0; i < spinPoint.childCount; i++)
        {
            meleeWeapons[i] = spinPoint.GetChild(i);
        }

        originColor = sr.color;
        hitColor = sr.color + new Color(0.5f, 0, 0, -0.8f);

        ImmuneLayerNum = LayerMask.NameToLayer("Immune");
        PlayerLayerNum = LayerMask.NameToLayer("Player");

        Life = maxLife;
        MeleeCount = 1;

        onDash = true;
    }

    private void Start()
    {
        StartCoroutine(GetClosestEnemyPosition());
        StartCoroutine(OnFire());
    }

    private void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position + Time.deltaTime * Speed * direction);
    }
    private void Update()
    {
        // transform.Translate(Time.deltaTime * Speed * direction);
        
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.performed += Move_performed;
        action.Player.Move.canceled += Move_performed;
        action.Player.Dash.performed += Dash_performed;
    }

    

    private void OnDisable()
    {
        action.Player.Dash.canceled -= Dash_performed;
        action.Player.Move.canceled -= Move_performed;
        action.Player.Move.performed -= Move_performed;
        action.Player.Disable();
    }

    /// <summary>
    /// 사정거리 내부에 적이 들어올 시 List 에 추가
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Coin coin = collision.gameObject.GetComponent<Coin>();
            Exp += coin.ExpPoint;
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Life--;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Life--;
    }

    /// <summary>
    /// 키보드로 방향을 Vector2 형식으로 받을 수 있고 나중에 추가적인 행동을 설정할 수 있도록 virtual 로 설정
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected virtual void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().normalized;

        FlipSpriteAndPosition();

        Speed = speed + AddedSpeed;
    }

    

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if (onDash)
        {
            StartCoroutine(Dash());
        }
    }

    void SetMeleeWeapon()
    {
        for (int i = 0; i < MeleeCount; i++)
        {
            meleeWeapons[i].gameObject.SetActive(true);
            float angle = 360 / MeleeCount;
            meleeWeapons[i].localPosition = Quaternion.Euler(0, 0, angle * i) * (Vector3.right * 2.0f);
            meleeWeapons[i].right = meleeWeapons[i].position - transform.position;
        }
    }

    Transform FIndClosestEnemy()
    {
        Transform ClosestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var enemy in enemiesInShootingZone)
        {
            if (!enemy.gameObject.activeSelf)
                enemiesInShootingZone.Remove(enemy);
            float distance = Vector3.Distance(enemy.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                ClosestEnemy = enemy;
            }
        }

        return ClosestEnemy;
    }

    

    void OnHit()
    {
        StartCoroutine(Hit());
    }

    void OnDie()
    {
        action.Player.Disable();

        coll.enabled = false;

        onDie?.Invoke();
    }

    /// <summary>
    /// 매 주기마다 가장 가까운 적의 위치를 갱신하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator GetClosestEnemyPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            // 사정거리 내부에 적이 있을 경우에만 실행
            if (enemiesInShootingZone.Count > 0)
            {
                enemyInRange = true;
                nearestEnemy = FIndClosestEnemy();
            } else
                enemyInRange = false;
        }
    }

    IEnumerator OnFire()
    {
        while (true)
        {
            if (enemyInRange) // 사정거리 내부에 적이 있을경우에만 사격
                StartCoroutine(Fire());
            yield return new WaitForSeconds(fireInterval);
        }
    }

    /// <summary>
    /// enemiesInShootingZone 내에서 가장 가까운 적을 찾는다
    /// </summary>
    /// <returns>가장 가까운 적의 Transform</returns>
    

    IEnumerator Fire()
    {
        for(int i = 0; i < barrageCount; i++)
        {
            Bullet bullet = Factory.Instance.GetBullet(firePoint.position);
            float bulletAngle = Vector3.SignedAngle(Vector3.up, nearestEnemy.position - firePoint.position, Vector3.forward);
            bullet.transform.Rotate(bulletAngle * Vector3.forward);
            yield return new WaitForSeconds(barrageInterval);
        }
    }

    

    IEnumerator Hit()
    {
        float elapsedTime = 0.0f;

        gameObject.layer = ImmuneLayerNum;

        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            sr.color = new Color(1, 1, 1, (Mathf.Cos(elapsedTime * 30.0f) + 1.0f) * 0.5f);

            yield return null;
        }

        sr.color = Color.white;
        gameObject.layer = PlayerLayerNum;
    }

    // 대쉬 기능
    IEnumerator Dash()
    {
        float elapsedTime = 0.0f;
        float orgSpeed = Speed;
        Speed = 15.0f;

        gameObject.layer = ImmuneLayerNum; // 무적 레이어로 변경

        StartCoroutine(DashCoolDown()); // 대쉬 쿨타임 계산 시작

        animator.SetTrigger(DashParameter_Hash);

        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;

            Speed = Mathf.Lerp(Speed, orgSpeed, Time.deltaTime * 2.0f);

            yield return null;
        }

        gameObject.layer = PlayerLayerNum; // 무적 시간 종료

        Speed = orgSpeed;
    }

    IEnumerator DashCoolDown()
    {
        CurrentDashCoolDown = 0.0f;
        onDash = false;

        while (currentDashCoolDown < dashCoolDown)
        {
            CurrentDashCoolDown += Time.deltaTime;

            yield return null;
        }

        onDash = true;
    }

    void FlipSpriteAndPosition()
    {
        // 플레이어가 왼쪽으로 향하면 왼쪽을보고 오른쪽을 향하면 오른쪽을 본다.
        if ((direction.x < 0.0f && !isFlipped) || (direction.x > 0.0f && isFlipped))
        {
            sr.flipX = !sr.flipX;
            isFlipped = !isFlipped;
            srWeapon.flipX = !srWeapon.flipX; // 무기 스프라이트 좌우 반전
            srWeapon.sortingOrder = sr.flipX ? 2 : 0; // 캐릭터가 오른쪽을 보면 캐릭터보다 아래에 그리고 왼쪽을 보면 위에 그린다
            // firePoint.localPosition = sr.flipX ? new Vector3(-0.7f, -0.1f) : new Vector3(0.7f, -0.1f);  // 무기 스프라이트 방향에 따라 firepoint 위치를 바꾼다.
            dashEffectTransform.localPosition *= -1;
            dashEffectSr.flipX = !dashEffectSr.flipX;
        }
    }

#if UNITY_EDITOR
    public void Test_LevelUp()
    {
        Exp += maxExp;
    }

    public void Test_Die()
    {
        Life = 0;
    }

    public void Test_Dash()
    {
        StartCoroutine(Dash());
    }

    private void OnDrawGizmos()
    {
        if (enemyInRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(nearestEnemy.position, new Vector3(1, 1));
        }

    }
#endif
}
