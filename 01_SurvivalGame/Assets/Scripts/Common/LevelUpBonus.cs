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

    // Panel ���� List ���� Bonus �� ������ �� �ߺ������� �����ϱ� ���� ��.
    public bool isSelected = false;

    public LevelUpBonus(Stat stat, int value)
    {
        this.stat = stat;
        this.value = value;
        Texture2D texture = Resources.Load<Texture2D>($"{stat.ToString()}");

        if (texture == null)
        {
            Debug.LogWarning("LevelUpBonus �ʱ�ȭ �� Texture ���� �б� ����.");
            return;
        }

        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
