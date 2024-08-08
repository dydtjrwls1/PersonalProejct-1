using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LevelUp 시 선택할 수 있는 보너스 들을 모아놓은 스크립트
// 선택된 Bonus 는 List 에서 제거된다.
public class LevelUpSlots : SingleTon<LevelUpSlots>
{
    public List<LevelUpBonus> bonusList;

    private void Awake()
    {
        bonusList = new List<LevelUpBonus>();
    }

    private void Start()
    {
        InitSlotList();
    }

    void InitSlotList()
    {
        bonusList.Clear();
        AddBonusSlots(LevelUpBonus.Stat.MeleeAttack, 5);
        AddBonusSlots(LevelUpBonus.Stat.RangeAttack, 5);
        AddBonusSlots(LevelUpBonus.Stat.MeleeCount, 5);
        AddBonusSlots(LevelUpBonus.Stat.RangeCount, 5);
        AddBonusSlots(LevelUpBonus.Stat.Speed, 5);
        AddBonusSlots(LevelUpBonus.Stat.Heal, 5);
    }

    void AddBonusSlots(LevelUpBonus.Stat stat, int count)
    {
        for (int i = 0; i < count; i++)
            bonusList.Add(new LevelUpBonus(stat, 1));
    }

    public void RemoveSlot(LevelUpBonus bonus)
    {
        bonusList.Remove(bonus);
    }
}
