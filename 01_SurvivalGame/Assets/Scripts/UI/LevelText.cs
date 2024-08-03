using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    TextMeshProUGUI levelText;

    int level;

    int Level
    {
        get => level;
        set
        {
            level = value;
            levelText.text = $"Level   {level.ToString()}";
        }
    }

    private void Awake()
    {
        levelText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Level = 1;
        PlayerBase player = GameManager.Instance.Player;
        player.levelUpAction += (currentLevel) => Level = currentLevel;
    }


}
