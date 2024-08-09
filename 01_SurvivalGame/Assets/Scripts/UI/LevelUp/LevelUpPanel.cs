using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    int slotCount = 3;

    List<LevelUpBonus> bonusList;

    LevelUpBoard[] levelUpBoards;

    Button[] buttons; 


    private void Awake()
    {
        levelUpBoards = GetComponentsInChildren<LevelUpBoard>();
        buttons = GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        bonusList = LevelUpSlots.Instance.bonusList;
        GameManager.Instance.Player.levelUpAction += RefreshSlots;
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => ButtonClicked(button));
        }
    }

    /// <summary>
    /// 레벨업 슬롯을 갱신한다.
    /// </summary>
    /// <param name="_"></param>
    void RefreshSlots(int _)
    {
        for (int i = 0; i < slotCount; i++)
        {
            int index = 0;
            int warning = 0;

            do
            {
                index = Random.Range(0, bonusList.Count);
                warning++;
                if (warning > 100)
                {
                    Debug.LogWarning("warning! // LevelUpPanel 에서 슬롯 부족으로 무한 루프.");
                    break;
                }
            } while (bonusList[index].isSelected); // levelUpBonus 클래스의 isSelected 가 true 일 경우 index 를 다시 뽑는다.

            levelUpBoards[i].Bonus = bonusList[index];
        }
        Time.timeScale = 0.0f;
    }
    
    void ButtonClicked(Button button)
    {
        LevelUpBonus bonus = button.GetComponent<LevelUpBoard>().Bonus;

        foreach (var board in levelUpBoards)
        {
            board.Bonus.isSelected = false;
        }

        PlayerBase player = GameManager.Instance.Player;

        switch (bonus.stat)
        {
            case LevelUpBonus.Stat.MeleeAttack:
                player.MeleePower += bonus.value;
                break;
            case LevelUpBonus.Stat.RangeAttack:
                player.RangePower += bonus.value;
                break;
            case LevelUpBonus.Stat.MeleeCount:
                player.MeleeCount++;
                break;
            case LevelUpBonus.Stat.RangeCount:
                player.BarrageCount++;
                break;
            case LevelUpBonus.Stat.Speed:
                player.AddedSpeed += bonus.value;
                break;
            case LevelUpBonus.Stat.Heal:
                player.Life++;
                break;
        }
        bonusList.Remove(bonus);

        GetComponentInParent<Animator>().SetTrigger("EndSelect");

        Time.timeScale = 1.0f;
    }
}
