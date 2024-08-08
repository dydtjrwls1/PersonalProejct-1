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
                if(warning > 100)
                {
                    Debug.LogWarning("warning! // 나중에 슬롯이 선택될 때 List 에서 제거되도록 설정.");
                    break;
                }
            } while (bonusList[index].isSelected); // levelUpBonus 클래스의 isSelected 가 true 일 경우 index 를 다시 뽑는다.

            levelUpBoards[i].Bonus = bonusList[index];
        }
    }
    
    void ButtonClicked(LevelUpBonus bonus)
    {
        foreach (var board in levelUpBoards)
        {
            board.Bonus.isSelected = false;
        }

        bonusList.Remove(bonus);

        GetComponentInParent<Animator>().SetTrigger("EndSelect");
    }
}
