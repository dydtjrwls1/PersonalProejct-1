using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skeleton : EnemyBase
{
    // ���� ���� �� true
    bool onPattern = false;

    // ĳ���Ϳ��� �ǽð� �Ÿ��� ������ ����
    float distance;

    // ��������Ʈ ���� ��ȭ �ð�
    float duration = 1.5f;

    public AnimationCurve curve;

    protected override void Update()
    {
        base.Update();

        distance = Vector3.Distance(transform.position, player.position);

        // ���� ������ �ƴϰ� �÷��̾�� �Ÿ��� 5 ���� ���� ��� ���� ����
        if (!onPattern && distance < 5.0f)
        {
            onPattern = true;
            StartCoroutine(RangeAttackPattern());
        }
    }

    IEnumerator RangeAttackPattern()
    {
        // ���� �ӵ��� �����ӵ��� �������� ���� ����ӵ��� �ӽ÷� �����ϰ� 0���� �Ѵ�.
        float orgSpeed = speed;
        speed = 0;


        float elapsedTime = 0.0f;
        Color targetColor = Color.red;
        Color startColor = sr.color;

        // duration ��ŭ�� �ð����� ������ �ȴ�.
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            sr.color = Color.Lerp(startColor, targetColor, curve.Evaluate(t));

            yield return null;
        }

        EnemyBullet bullet = Factory.Instance.GetEnemyBullet(transform.position);
        bullet.setDestination(player.position);
        sr.color = Color.white;

        onPattern = false;
        speed = orgSpeed;
    }
}
