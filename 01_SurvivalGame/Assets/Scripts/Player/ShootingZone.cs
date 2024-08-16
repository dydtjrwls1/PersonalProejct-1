using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootingZone : MonoBehaviour
{
    public Action<Transform> onEnemyEnter;

    public Action<Transform> onEnemyExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            onEnemyEnter?.Invoke(collision.gameObject.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            onEnemyExit?.Invoke(collision.gameObject.transform);
    }
}
