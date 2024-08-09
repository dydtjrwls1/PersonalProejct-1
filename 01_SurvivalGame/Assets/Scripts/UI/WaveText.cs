using System.Collections;
using TMPro;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    TextMeshProUGUI waveText;

    private void Awake()
    {
        waveText = GetComponent<TextMeshProUGUI>();
        waveText.color = new Color(1, 1, 1, 0.01f);
    }

    private void Start()
    {
        StartCoroutine(ActionAnimate());
    }

    IEnumerator ActionAnimate()
    {
        float elapsedTime = 0.0f;

        while (waveText.color.a > 0.00001f)
        {
            elapsedTime += Time.deltaTime;
            Color currentColor = waveText.color;
            float sinValue = Mathf.Sin(elapsedTime - (Mathf.PI * 0.5f));
            currentColor.a = (sinValue + 1.0f) * 0.5f;
            waveText.color = currentColor;

            yield return null;
        }
    }
}
