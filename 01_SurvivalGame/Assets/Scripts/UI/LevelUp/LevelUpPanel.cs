using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    List<LevelUpBonus> bonusList;

    LevelUpBonus meleeAtkSlot;
    LevelUpBonus rangeAtkSlot;
    LevelUpBonus meleeCountSlot;
    LevelUpBonus rangeCountSlot;
    LevelUpBonus healSlot;

    private void Awake()
    {
        
    }

    private void Start()
    {
    }

    void AddBonusSlots(LevelUpBonus slot, int count)
    {
        for(int i = 0; i < count; i++)
            bonusList.Add(slot);
    }
}
