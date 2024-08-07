using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    List<LevelUpBonus> bonusList;

    private void Awake()
    {
        
    }

    private void Start()
    {
        LevelUpBonus slot = new LevelUpBonus(LevelUpBonus.Stat.Speed, 1, "speed");
        
    }

    void AddBonusSlots(LevelUpBonus slot, int count)
    {
        for(int i = 0; i < count; i++)
            bonusList.Add(slot);
    }
}
