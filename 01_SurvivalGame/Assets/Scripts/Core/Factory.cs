using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : SingleTon<Factory>
{
    StrongZombieObjectPool strongZombie;
    ZombieObjectPool zombie;
    BulletObjectPool bullet;
    CoinObjectPool coin;

    protected override void OnInitialize()
    {
        strongZombie = GetComponentInChildren<StrongZombieObjectPool>();
        if (strongZombie != null)
            strongZombie.Initialize();

        zombie = GetComponentInChildren<ZombieObjectPool>();
        if(zombie != null)
            zombie.Initialize();

        bullet = GetComponentInChildren<BulletObjectPool>();
        if (bullet != null)
            bullet.Initialize();

        coin = GetComponentInChildren<CoinObjectPool>();
        if (coin != null)
            coin.Initialize();
    }

    public StrongZombie GetStrongZombie(Vector3? position = null, float angle = 0.0f)
    {
        return strongZombie.GetObject(position, new Vector3(0, 0, angle));
    }

    public Zombie GetZombie(Vector3? position = null, float angle = 0.0f)
    {
        return zombie.GetObject(position, new Vector3(0, 0, angle));
    }

    public Bullet GetBullet(Vector3? position = null,float angle = 0.0f)
    {
        return bullet.GetObject(position, new Vector3(0, 0, angle));
    }

    public Coin GetCoin(Vector3? position = null)
    {
        return coin.GetObject(position);
    }
}
