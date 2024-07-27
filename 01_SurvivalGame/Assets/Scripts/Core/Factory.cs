using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : SingleTon<Factory>
{
    ZombieObjectPool zombie;

    protected override void OnInitialize()
    {
        zombie = GetComponentInChildren<ZombieObjectPool>();
        if(zombie != null)
            zombie.Initialize();
    }

    public Zombie GetZombie(Vector3? position = null, float angle = 0.0f)
    {
        return zombie.GetObject(position, new Vector3(0, 0, angle));
    }
}
