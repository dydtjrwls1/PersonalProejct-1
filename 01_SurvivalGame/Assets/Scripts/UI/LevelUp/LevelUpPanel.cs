using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    LevelUpBonus[] currentSlots = new LevelUpBonus[3];

    LevelUpSlots levelUpslot;

    LevelUpBoard[] levelUpBoards;

    private void Awake()
    {
        levelUpslot = FindAnyObjectByType<LevelUpSlots>();
        levelUpBoards = GetComponentsInChildren<LevelUpBoard>();
    }

    private void Start()
    {
        GameManager.Instance.Player.levelUpAction += RefreshSlots;
    }

    void RefreshSlots(int _)
    {
        for (int i = 0; i < currentSlots.Length; i++)
        {
            int index = Random.Range(0, levelUpslot.bonusList.Count);
            currentSlots[i] = levelUpslot.bonusList[index];
            levelUpslot.bonusList.RemoveAt(index);

            Image boardImage = levelUpBoards[i].transform.GetChild(0).GetComponent<Image>();
            boardImage.sprite = currentSlots[i].sprite;

            TextMeshProUGUI boardText = levelUpBoards[i].GetComponentInChildren<TextMeshProUGUI>();
            boardText.text = $"{currentSlots[i].stat.ToString()} + {currentSlots[i].value}";
        }
    }
}
