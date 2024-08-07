using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSpace : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.Player.levelUpAction += (_) => animator.SetTrigger("LevelUp");
    }
}
