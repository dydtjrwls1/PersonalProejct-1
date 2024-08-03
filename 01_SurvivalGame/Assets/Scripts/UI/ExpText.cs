using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpText : MonoBehaviour
{
    TextMeshProUGUI expText;

    int exp;

    int maxExp = 5;

    int Exp
    {
        get => exp;
        set
        {
            exp = value;
            expText.text = $"{exp} / {maxExp}";
        }
    }

    private void Awake()
    {
        expText = GetComponent<TextMeshProUGUI>();
        Exp = 0;
    }

    private void Start()
    {
        PlayerBase player = GameManager.Instance.Player;
        player.expUpAction += (currentExp, currentMaxExp) =>
        {
            maxExp = currentMaxExp;
            Exp = currentExp;
        };
    }
}
