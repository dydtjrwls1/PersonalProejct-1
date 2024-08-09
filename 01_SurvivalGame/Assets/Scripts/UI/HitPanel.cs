using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPanel : MonoBehaviour
{
    Color orgColor;

    PlayerBase player;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        orgColor = new Color(1, 0, 0, 0.4f);
    }

    private void Start()
    {
        player = GameManager.Instance.Player;

        player.onHit += HitEffect;
    }

    void HitEffect( )
    {
        StartCoroutine(HitEffectAction());
    }

    IEnumerator HitEffectAction()
    {
        float elapsedTime = 0.0f;
        image.enabled = true;
        image.color = orgColor;

        while (image.color.a > 0.001f)
        {
            elapsedTime += Time.deltaTime;

            image.color = new Color(1, 0, 0, (Mathf.Cos(elapsedTime * 10.0f) + 1.0f) * 0.2f);

            yield return null;
        }

        image.enabled = false;
    }
}
