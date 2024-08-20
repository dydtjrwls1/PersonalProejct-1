using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    TextMeshProUGUI timeText;

    float elapsedTime;

    int minutes = 0;

    int seconds = 0;

    private void OnEnable()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        seconds = Mathf.RoundToInt(elapsedTime);
        if(seconds == 60)
        {
            minutes += 1;
            elapsedTime = 0;
            seconds = 0;
            if(minutes % 2 == 0)
                GameManager.Instance.Wave++;
        }

        timeText.text = $"{minutes:D2} : {seconds:D2}";
    }
}
