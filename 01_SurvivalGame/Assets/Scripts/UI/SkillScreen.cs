using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillScreen : MonoBehaviour
{
    PlayerBase player;

    Image dashIcon;

    private void Awake()
    {
        dashIcon = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        dashIcon.fillAmount = player.CurrentDashCoolDown == 0.0f? 0.0f : player.CurrentDashCoolDown / player.dashCoolDown;
    }
}
