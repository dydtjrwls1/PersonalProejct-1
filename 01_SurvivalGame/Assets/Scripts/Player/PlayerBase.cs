using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
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

    // 플레이어 현재 속도
    float currentSpeed = 0.0f;

    // 플레이어 스프라이트의 좌우반전 확인용
    bool isFlipped = false;

    // 플레이어 사정거리 내에 적이 있는지 여부
    bool enemyInRange = false;

    int exp = 0;

    PlayerInputAction action;

    // 플레이어 애니메이션 컨트롤러
    Animator animator;
    
    // 플레이어 스프라이트
    SpriteRenderer sr;

    Rigidbody2D rb;

    // 가장 가까운 적의 Transform
    Transform nearestEnemy;

    // 사정거리 내의 적들의 Transform 배열
    List<Transform> enemiesInShootingZone = new List<Transform>();

    // 플레이어 진행 방향
    Vector2 direction;

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

    public int Exp
    {
        get => exp;
        set
        {
            exp = value;
        }
    }

    // 애니메이션 제어를 위한 Speed 파라미터의 해쉬번호
    readonly int SpeedParameter_Hash = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Awake()
    {
        // 필요한 컴포넌트 초기화
        action = new PlayerInputAction();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
        action.Player.Move.canceled += Move_canceled;
    }

    private void OnDisable()
    {
        action.Player.Move.canceled -= Move_canceled;
        action.Player.Move.performed -= Move_performed;
        action.Player.Disable();
    }

    /// <summary>
    /// 사정거리 내부에 적이 들어올 시 List 에 추가
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyBase>() != null)
        {
            enemiesInShootingZone.Add(collision.gameObject.transform);
        }
        
    }

    /// <summary>
    /// 사정거리에서 벗어난 적은 List 에서 제외
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyBase>() != null)
        {
            enemiesInShootingZone.Remove(collision.gameObject.transform);
        }
    }

    /// <summary>
    /// 키보드로 방향을 Vector2 형식으로 받을 수 있고 나중에 추가적인 행동을 설정할 수 있도록 virtual 로 설정
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected virtual void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().normalized;

        // 플레이어가 왼쪽으로 향하면 왼쪽을보고 오른쪽을 향하면 오른쪽을 본다.
        if ((direction.x < 0.0f && !isFlipped) || (direction.x > 0.0f && isFlipped))
        {
             sr.flipX = !sr.flipX;
            isFlipped = !isFlipped;
            srWeapon.flipX = !srWeapon.flipX; // 무기 스프라이트 좌우 반전
            srWeapon.sortingOrder = sr.flipX ? 2 : 0; // 캐릭터가 오른쪽을 보면 캐릭터보다 아래에 그리고 왼쪽을 보면 위에 그린다
            firePoint.localPosition = sr.flipX ? new Vector3(-0.7f, -0.1f) : new Vector3(0.7f, -0.1f);  // 무기 스프라이트 방향에 따라 firepoint 위치를 바꾼다.
        }

        Speed = speed;
    }

    /// <summary>
    /// 키보드 버튼을 뗀 순간 방향은 0 이 되는 함수 
    /// </summary>
    /// <param name="context"></param>
    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Speed = 0.0f;
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
            if(enemiesInShootingZone.Count > 0)
            {
                enemyInRange = true;
                nearestEnemy = FIndClosestEnemy();
            } else
            {
                enemyInRange = false;
            }
        }
    }

    IEnumerator OnFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            // 사정거리 내부에 적이 있을경우에만 사격
            if (enemyInRange)
                Fire();
        }
    }

    /// <summary>
    /// enemiesInShootingZone 내에서 가장 가까운 적을 찾는다
    /// </summary>
    /// <returns>가장 가까운 적의 Transform</returns>
    Transform FIndClosestEnemy()
    {
        Transform ClosestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var enemy in enemiesInShootingZone)
        {
            float distance = Vector3.Distance(enemy.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                ClosestEnemy = enemy;
            }
        }

        return ClosestEnemy;
    }

    void Fire()
    {
        Bullet bullet = Factory.Instance.GetBullet(firePoint.position);
        float bulletAngle = Vector3.SignedAngle(Vector3.up, nearestEnemy.position - firePoint.position, Vector3.forward);
        bullet.transform.Rotate(bulletAngle * Vector3.forward);
    }

    private void OnDrawGizmos()
    {
        if (enemyInRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(nearestEnemy.position, new Vector3(1, 1));
        }

    }

}
