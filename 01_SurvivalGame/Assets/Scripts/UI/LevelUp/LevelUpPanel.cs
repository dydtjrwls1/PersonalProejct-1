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
                index = Random.Range(0, levelUpslot.bonusList.Count);
                warning++;
                if(warning > 100)
                {
                    Debug.LogWarning("warning! // ���߿� ������ ���õ� �� List ���� ���ŵǵ��� ����.");
                    break;
                }
            } while (levelUpslot.bonusList[index].isSelected); // levelUpBonus Ŭ������ isSelected �� true �� ��� index �� �ٽ� �̴´�.

            levelUpBoards[i].Bonus = levelUpslot.bonusList[index];
        }
    }

 
}
