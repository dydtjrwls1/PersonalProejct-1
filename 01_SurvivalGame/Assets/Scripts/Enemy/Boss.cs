using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    // �÷��̾�� ���� ������ �Ÿ�
    float distance;

    // ���� ���� �� �ӵ�
    float dashSpeed = 10.0f;

    bool onPattern = false;

    // player ���� ���Ͻ� �÷��̾ ������ �ð�
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

        // ���� ���� ��ġ�� ���� �÷��̾��� ��ġ���� ���������� 15
        Vector3 appearancePosition = player.position + Vector3.right * 15.0f;
        transform.position = appearancePosition;

        // ���� �ִϸ��̼� ����
        StartCoroutine(AppearanceAction());
    }

    protected override void Update()
    {
        base.Update();

        if (!onPattern)
        {
            // �÷��̾���� �Ÿ� �ǽð����� ����
            distance = Vector3.Distance(transform.position, player.position);

            // ������ ���������� �ʰ� �Ÿ��� 5 ���� ������� ���� ����
            if (distance < 5.0f)
                StartCoroutine(Pattern());
        }
    }

    // ���� ��� �� ���ӿ����� �Ǿ��ϱ� ������ �÷��̾� ��� �̺�Ʈ ȣ��
    protected override void Die()
    {
        base.Die();
        GameManager.Instance.Player.onDie?.Invoke();
    }

    /// <summary>
    /// ���� ���� �ִϸ��̼� (���� 5��)
    /// </summary>
    /// <returns></returns>
    IEnumerator AppearanceAction()
    {
        // �ִϸ��̼� ��� �ð� ���� ������ �������� �ʵ��� �ӵ� 0 ���� ����
        float orgSpeed = speed;
        speed = 0.0f;

        // �ִϸ��̼� ��� ����
        animator.SetTrigger("Appear");

        // trigger �ߵ� ���Ŀ��� ���� �ִϸ��̼� Ŭ�� ������ �������� ���ؼ� �� ������ ��ٸ���.
        yield return null;

        // �ִϸ��̼� ��� ���� �÷��̾ ������ �� ������ rigidbody ����
        rb.simulated = false;

        // ���� �ִϸ��̼� (Boss_Appear) �� ��� �ð� ����
        float AppearAnimationClipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        // ī�޶� ������ ����
        StartCoroutine(CameraMoving(AppearAnimationClipLength));

        // �ִϸ��̼� ��� �ð����� ���
        yield return new WaitForSeconds(AppearAnimationClipLength);

        // �ִϸ��̼� ��� �� �ӵ� �� rigidbody ���� ������ �ǵ�����
        speed = orgSpeed;
        rb.simulated = true;
    }

    // ���� ī�޶�� ������ ī�޶� ���� ���� �ִϸ��̼� �ð�(5��) ��ŭ ��ȯ�Ѵ�.
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
        float orgSpeed = speed; // ���� �ӵ� ����
        speed = 0.0f;           // ����
        onPattern = true;       // ������ ���۵����ν� �� ����

        targetSr.color = Color.red;

        // Line ����
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer ����
        lineRenderer.startWidth = 0.1f;  // ���� ���� ���� �β�
        lineRenderer.endWidth = 0.1f;    // ���� �� ���� �β�
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));  // ���� �⺻ ���� ����
        lineRenderer.startColor = Color.red;  // ���� ���� ���� ����
        lineRenderer.endColor = Color.red;    // ���� �� ���� ����

        // 1.5 �� ���� �÷��̾� ��ġ�� line �׸���
        float elapsedTime = 0.0f;

        while (elapsedTime < playerChaseTime)
        {
            targetPoint.position = player.position;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.position);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Vector3 destination = player.position; // ������ ��ġ
        Vector3 direction = (destination - transform.position).normalized; // ������ ����

        elapsedTime = 0.0f;

        // 0.5�� ���
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // targetPoint �����
        targetSr.color = Color.clear;

        // ���� �ӵ��� ��ǥ ��ġ�� ����
        float distanceToDestination = Vector3.Distance(destination, transform.position);

        while (distanceToDestination > 0.1f)
        {
            sr.flipX = sr.flipX;

            speed = dashSpeed;
            Direction = direction;

            distanceToDestination = Vector3.Distance(destination, transform.position);

            yield return null;
        }

        // ���� ����
        speed = orgSpeed;
        onPattern = false;
        Destroy(lineRenderer);
    }
}
