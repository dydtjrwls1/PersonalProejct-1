using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    int slotCount = 3;

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
                index = Random.Range(0, levelUpslot.bonusList.Count);
                warning++;
                if(warning > 100)
                {
                    Debug.LogWarning("warning! // 나중에 슬롯이 선택될 때 List 에서 제거되도록 설정.");
                    break;
                }
            } while (levelUpslot.bonusList[index].isSelected); // levelUpBonus 클래스의 isSelected 가 true 일 경우 index 를 다시 뽑는다.

            levelUpBoards[i].Bonus = levelUpslot.bonusList[index];
        }
    }

 
}
