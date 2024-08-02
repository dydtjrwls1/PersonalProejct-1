using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : RecycleObject
{
    int expPoint = 1;

    public int ExpPoint
    {
        get => expPoint;
        set { expPoint = value; }
    }
}
