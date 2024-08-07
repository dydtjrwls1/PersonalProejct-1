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

    // Panel 에서 List 내의 Bonus 를 선택할 때 중복선택을 방지하기 위한 값.
    public bool isSelected = false;

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
}
