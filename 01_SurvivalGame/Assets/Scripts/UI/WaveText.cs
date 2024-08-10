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
        StartCoroutine(ActionAnimate(1));
        GameManager.Instance.onWaveChange += (currentWave) => StartCoroutine(ActionAnimate(currentWave));
    }

    IEnumerator ActionAnimate(int currentWave)
    {
        float elapsedTime = 0.0f;
        waveText.text = $"Wave {currentWave}";

        waveText.color = new Color(1, 1, 1, 0.01f);

        while (waveText.color.a > 0.0000001f)
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
