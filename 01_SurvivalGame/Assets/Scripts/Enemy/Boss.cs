using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    // 플레이어와 보스 사이의 거리
    float distance;

    // 돌진 패턴 시 속도
    float dashSpeed = 10.0f;

    bool onPattern = false;

    // player 추적 패턴시 플레이어를 추적할 시간
    float playerChaseTime = 1.5f;

    SpriteRenderer targetSr;

    Transform targetPoint;

    protected override void Awake()
    {
        base.Awake();
        targetSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        targetPoint = transform.GetChild(0);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 보스 등장 위치는 현재 플레이어의 위치에서 오른쪽으로 15
        Vector3 appearancePosition = player.position + Vector3.right * 15.0f;
        transform.position = appearancePosition;

        // 등장 애니메이션 시작
        StartCoroutine(AppearanceAction());
    }

    protected override void Update()
    {
        base.Update();

        if (!onPattern)
        {
            // 플레이어와의 거리 실시간으로 측정
            distance = Vector3.Distance(transform.position, player.position);

            // 패턴이 실행중이지 않고 거리가 5 보다 작을경우 패턴 실행
            if (distance < 5.0f)
                StartCoroutine(Pattern());
        }
    }

    // 보스 사망 시 게임오버가 되야하기 때문에 플레이어 사망 이벤트 호출
    protected override void Die()
    {
        base.Die();
        GameManager.Instance.Player.onDie?.Invoke();
    }

    /// <summary>
    /// 보스 등장 애니메이션 (길이 5초)
    /// </summary>
    /// <returns></returns>
    IEnumerator AppearanceAction()
    {
        // 애니메이션 재생 시간 동안 보스가 움직이지 않도록 속도 0 으로 조절
        float orgSpeed = speed;
        speed = 0.0f;

        // 애니메이션 재생 시작
        animator.SetTrigger("Appear");

        // trigger 발동 직후에는 다음 애니메이션 클립 정보를 감지하지 못해서 한 프레임 기다린다.
        yield return null;

        // 애니메이션 재생 동안 플레이어가 공격할 수 없도록 rigidbody 끄기
        rb.simulated = false;

        // 현재 애니메이션 (Boss_Appear) 의 재생 시간 변수
        float AppearAnimationClipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        // 카메라 움직임 시작
        StartCoroutine(CameraMoving(AppearAnimationClipLength));

        // 애니메이션 재생 시간동안 대기
        yield return new WaitForSeconds(AppearAnimationClipLength);

        // 애니메이션 재생 후 속도 와 rigidbody 원래 값으로 되돌리기
        speed = orgSpeed;
        rb.simulated = true;
    }

    // 메인 카메라와 보스용 카메라를 보스 등장 애니메이션 시간(5초) 만큼 전환한다.
    IEnumerator CameraMoving(float time)
    {
        Camera[] cameras = FindObjectsOfType<Camera>(true);

        cameras[0].enabled = !cameras[0].enabled;
        cameras[1].gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        cameras[0].enabled = !cameras[0].enabled;
        cameras[1].gameObject.SetActive(false);
    }

    IEnumerator Pattern()
    {
        float orgSpeed = speed; // 원래 속도 저장
        speed = 0.0f;           // 정지
        onPattern = true;       // 패턴이 시작됨으로써 값 변경

        targetSr.color = Color.red;

        // Line 생성
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer 설정
        lineRenderer.startWidth = 0.1f;  // 선의 시작 지점 두께
        lineRenderer.endWidth = 0.1f;    // 선의 끝 지점 두께
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));  // 선의 기본 재질 설정
        lineRenderer.startColor = Color.red;  // 선의 시작 지점 색상
        lineRenderer.endColor = Color.red;    // 선의 끝 지점 색상

        // 1.5 초 동안 플레이어 위치로 line 그리기
        float elapsedTime = 0.0f;

        while (elapsedTime < playerChaseTime)
        {
            targetPoint.position = player.position;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.position);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Vector3 destination = player.position; // 접근할 위치
        Vector3 direction = (destination - transform.position).normalized; // 접근할 방향

        elapsedTime = 0.0f;

        // 0.5초 대기
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // targetPoint 숨기기
        targetSr.color = Color.clear;

        // 빠른 속도로 목표 위치로 접근
        float distanceToDestination = Vector3.Distance(destination, transform.position);

        while (distanceToDestination > 0.1f)
        {
            sr.flipX = sr.flipX;

            speed = dashSpeed;
            Direction = direction;

            distanceToDestination = Vector3.Distance(destination, transform.position);

            yield return null;
        }

        // 패턴 종료
        speed = orgSpeed;
        onPattern = false;
        Destroy(lineRenderer);
    }
}
