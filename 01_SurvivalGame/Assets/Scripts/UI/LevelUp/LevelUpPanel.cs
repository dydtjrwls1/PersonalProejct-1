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
            LevelUpBonus bonus = button.gameObject.GetComponent<LevelUpBoard>().Bonus;
            button.onClick.AddListener(() => ButtonClicked(bonus));
        }

        
    }

    /// <summary>
    /// ������ ������ �����Ѵ�.
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
                if(warning > 100)
                {
                    Debug.LogWarning("warning! // ���߿� ������ ���õ� �� List ���� ���ŵǵ��� ����.");
                    break;
                }
            } while (bonusList[index].isSelected); // levelUpBonus Ŭ������ isSelected �� true �� ��� index �� �ٽ� �̴´�.

            levelUpBoards[i].Bonus = bonusList[index];
        }
    }
    
    void ButtonClicked(LevelUpBonus bonus)
    {
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
                break;
            case LevelUpBonus.Stat.RangeCount:
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
    }
}
