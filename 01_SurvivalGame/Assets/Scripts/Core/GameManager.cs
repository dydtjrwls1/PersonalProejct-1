using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    PlayerBase player;

    public PlayerBase Player
    {
        get
        {
            if (player == null)
            {
                OnInitialize();
            }
            return player;
        }
        
    }
    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<PlayerBase>();
    }
}
