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
        MeleeCount,
        RangeCount,
        Speed,
        Heal
    };

    public Stat stat;

    public int value;

    public Sprite sprite;

    public LevelUpBonus(Stat stat, int value)
    {
        this.stat = stat;
        this.value = value;
        Texture2D texture = Resources.Load<Texture2D>($"{stat.ToString()}");

        if (texture == null)
        {
            Debug.LogWarning("LevelUpBonus 초기화 중 Texture 파일 읽기 실패.");
            return;
        }

        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public static LevelUpBonus GetMeleeAttack()
    {
        return new LevelUpBonus(Stat.MeleeAttack, 1);
    }

    public static LevelUpBonus GetRangeAttack()
    {
        return new LevelUpBonus(Stat.RangeAttack, 1);
    }

    public static LevelUpBonus GetMeleeCount()
    {
        return new LevelUpBonus(Stat.MeleeCount, 1);
    }

    public static LevelUpBonus GetRangeCount()
    {
        return new LevelUpBonus(Stat.RangeCount, 1);
    }

    public static LevelUpBonus GetSpeed()
    {
        return new LevelUpBonus(Stat.Speed, 1);
    }

    public static LevelUpBonus GetHeal()
    {
        return new LevelUpBonus(Stat.Heal, 1);
    }
}
