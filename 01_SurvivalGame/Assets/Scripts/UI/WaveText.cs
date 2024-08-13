using System.Collections;
using TMPro;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    TextMeshProUGUI waveText;

    Color transparentColor;

    int bossWave = 8;

    private void Awake()
    {
        waveText = GetComponent<TextMeshProUGUI>();
        transparentColor = Color.white - Color.black;
        waveText.color = transparentColor;
    }

    private void Start()
    {
        StartCoroutine(ActionAnimate(1));
        GameManager.Instance.onWaveChange += (currentWave) => StartCoroutine(ActionAnimate(currentWave));
    }

    IEnumerator ActionAnimate(int currentWave)
    {
        float elapsedTime = 0.0f;

        if (currentWave == bossWave)
            waveText.text = "Boss Wave";
        else
            waveText.text = $"Wave {currentWave}";


        waveText.color = transparentColor;

        while (elapsedTime < 6.5f)
        {
            elapsedTime += Time.deltaTime;
            Color currentColor = waveText.color;

            float sinValue = Mathf.Sin(elapsedTime - (Mathf.PI * 0.5f));
            currentColor.a = (sinValue + 1.0f) * 0.5f;

            waveText.color = currentColor;

            yield return null;
        }

        waveText.color = transparentColor;
    }
}
