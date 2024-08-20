using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    Button startButton;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        startButton = GetComponentInChildren<Button>();
        canvasGroup = GetComponent<CanvasGroup>();

        startButton.onClick.AddListener(GameStart);
    }

    private void Start()
    {
        Time.timeScale = 0.0f;
    }

    void GameStart()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        Time.timeScale = 1.0f;
    }
}
