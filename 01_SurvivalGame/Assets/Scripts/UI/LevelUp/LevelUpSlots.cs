using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSlots : MonoBehaviour
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
        AddBonusSlots(LevelUpBonus.GetMeleeAttack(), 5);
        AddBonusSlots(LevelUpBonus.GetRangeAttack(), 5);
        AddBonusSlots(LevelUpBonus.GetMeleeCount(), 5);
        AddBonusSlots(LevelUpBonus.GetRangeCount(), 5);
        AddBonusSlots(LevelUpBonus.GetSpeed(), 5);
        AddBonusSlots(LevelUpBonus.GetHeal(), 5);
        AddBonusSlots(LevelUpBonus.GetRangeAttack(), 5);
    }

    void AddBonusSlots(LevelUpBonus slot, int count)
    {
        for (int i = 0; i < count; i++)
            bonusList.Add(slot);
    }
}
