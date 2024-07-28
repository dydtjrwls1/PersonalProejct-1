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

    public Transform firePoint;

    // 플레이어 현재 속도
    float currentSpeed = 0.0f;

    bool isFlipped = false;

    bool enemyInRange = false;

    PlayerInputAction action;

    Animator animator;

    SpriteRenderer sr;

    Transform nearestEnemy;

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
    readonly int SpeedParameter_Hash = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Awake()
    {
        action = new PlayerInputAction();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(GetClosestEnemyPosition());
        StartCoroutine(OnFire());
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * Speed * direction);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyBase>() != null)
        {
            enemiesInShootingZone.Add(collision.gameObject.transform);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyBase>() != null)
        {
            enemiesInShootingZone.Add(collision.gameObject.transform);
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

        //if (!isFlipped && direction.x < 0.0f)
        //{
        //    sr.flipX = true;
        //    isFlipped = true;
        //}

        //if (direction.x > 0.0f)
        //{
        //    sr.flipX = false;
        //    isFlipped = false;
        //}

        if ((direction.x < 0.0f && !isFlipped) || (direction.x > 0.0f && isFlipped))
        {
            sr.flipX = !sr.flipX;
            isFlipped = !isFlipped;
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

            if(enemiesInShootingZone.Count > 0)
            {
                enemyInRange = true;

                nearestEnemy = FIndClosestEnemy();

                Debug.Log($"Neareast Enemy : {nearestEnemy.name}");
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
            yield return new WaitForSeconds(0.1f);
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
        Bullet bullet = Factory.Instance.GetBullet(firePoint.position, 0.0f);
        bullet.transform.LookAt(nearestEnemy.position);
    }
}
