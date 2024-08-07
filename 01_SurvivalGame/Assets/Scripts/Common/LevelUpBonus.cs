using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Stat stat;

    public int value;

    public Texture2D texture;

    public LevelUpBonus(Stat stat, int value, string fileName)
    {
        this.stat = stat;
        this.value = value;
        texture = Resources.Load<Texture2D>(fileName);

        if (texture == null)
        {
            Debug.LogWarning("LevelUpBonus 초기화 중 Texture 파일 읽기 실패.");
            return;
        }
    }

    public LevelUpBonus GetMeleeAttack()
    {
        return new LevelUpBonus(Stat.MeleeAttack, 1, "MeleeAttack");
    }

    public LevelUpBonus GetRangeAttack()
    {
        return new LevelUpBonus(Stat.RangeAttack, 1, "RangeAttack");
    }

    public LevelUpBonus GetMeleeCount()
    {
        return new LevelUpBonus(Stat.MeleeCount, 1, "MeleeCount");
    }

    public LevelUpBonus GetRangeCount()
    {
        return new LevelUpBonus(Stat.RangeCount, 1, "RangeCount");
    }

    public LevelUpBonus GetSpeed()
    {
        return new LevelUpBonus(Stat.Speed, 1, "Speed");
    }

    public LevelUpBonus GetHeal()
    {
        return new LevelUpBonus(Stat.Heal, 1, "Health");
    }
}
