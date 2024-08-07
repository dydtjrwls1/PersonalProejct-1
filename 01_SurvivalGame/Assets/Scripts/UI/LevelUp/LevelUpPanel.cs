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

    /// <summary>
    /// 레벨업 슬롯을 갱신한다.
    /// </summary>
    /// <param name="_"></param>
    void RefreshSlots(int _)
    {
        for (int i = 0; i < currentSlots.Length; i++)
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

            currentSlots[i] = levelUpslot.bonusList[index];
            levelUpslot.bonusList[index].isSelected = true; // isSelected를 True 로 바꿈으로써 더 이상 뽑히지 않게 한다.

            Image boardImage = levelUpBoards[i].transform.GetChild(0).GetComponent<Image>();
            boardImage.sprite = currentSlots[i].sprite;

            TextMeshProUGUI boardText = levelUpBoards[i].GetComponentInChildren<TextMeshProUGUI>();
            boardText.text = $"{currentSlots[i].stat.ToString()} + {currentSlots[i].value}";
        }
    }
}
