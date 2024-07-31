using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : RecycleObject
{
    PlayerBase player;

    int expPoint = 1;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        player.Exp += expPoint;
        Debug.Log(player.Exp);
    }
}
