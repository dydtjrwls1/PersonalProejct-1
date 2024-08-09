using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealPanel : MonoBehaviour
{
    float effectPlaySpeed = 10.0f;

    Color orgColor;

    PlayerBase player;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        orgColor = new Color(0, 1, 0, 0.4f);
    }

    private void Start()
    {
        player = GameManager.Instance.Player;

        player.onHeal += HealEffect;
    }

    void HealEffect()
    {
         StartCoroutine(HealEffectAction());
    }

    IEnumerator HealEffectAction()
    {
        float elapsedTime = 0.0f;
        image.enabled = true;
        image.color = orgColor;

        while (image.color.a > 0.001f)
        {
            elapsedTime += Time.deltaTime;

            image.color = new Color(0, 1, 0, (Mathf.Cos(elapsedTime * effectPlaySpeed) + 1.0f) * 0.2f);

            yield return null;
        }

        image.enabled = false;
    }
}
