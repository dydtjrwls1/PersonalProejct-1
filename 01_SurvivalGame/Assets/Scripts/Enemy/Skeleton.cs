using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skeleton : EnemyBase
{
    // 패턴 시전 시 true
    bool onPattern = false;

    // 캐릭터와의 실시간 거리를 저장할 변수
    float distance;

    // 스프라이트 색상 변화 시간
    float duration = 1.5f;

    public AnimationCurve curve;

    protected override void Update()
    {
        base.Update();

        distance = Vector3.Distance(transform.position, player.position);

        // 패턴 도중이 아니고 플레이어와 거리가 5 보다 작을 경우 패턴 실행
        if (!onPattern && distance < 5.0f)
        {
            onPattern = true;
            StartCoroutine(RangeAttackPattern());
        }
    }

    IEnumerator RangeAttackPattern()
    {
        // 이후 속도를 원래속도로 돌려놓기 위해 현재속도를 임시로 저장하고 0으로 한다.
        float orgSpeed = speed;
        speed = 0;


        float elapsedTime = 0.0f;
        Color targetColor = Color.red;
        Color startColor = sr.color;

        // duration 만큼의 시간동안 빨갛게 된다.
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
