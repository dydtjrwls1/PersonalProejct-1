using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LevelUp �� ������ �� �ִ� ���ʽ� ���� ��Ƴ��� ��ũ��Ʈ
// ���õ� Bonus �� List ���� ���ŵȴ�.
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
