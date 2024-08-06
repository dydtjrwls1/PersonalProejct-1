using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBonus
{
    public enum Stat
    {
        MeleeAttack = 0,
        RangeAttack,
        Speed,
        MeleeCount,
        RangeCount,
        Heal
    };

    Stat stat;

    public int value;

    LevelUpBonus(Stat stat, int value)
    {
        this.stat = stat;
        this.value = value;
    }
}
