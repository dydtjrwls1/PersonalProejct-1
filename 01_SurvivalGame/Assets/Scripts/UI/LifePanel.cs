using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePanel : MonoBehaviour
{
    PlayerBase player;

    Image[] lifeImages;

    
    private void Awake()
    {
        lifeImages = new Image[transform.childCount];
        for(int i = 0; i < lifeImages.Length; i++)
        {
            Transform child = transform.GetChild(i);
            lifeImages[i] = transform.GetComponent<Image>();
        }
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.lifeChange += LifeChange;
    }

    void LifeChange(int life)
    {
        for(int i = 0; i < lifeImages.Length; i++)
        {
            if (i < life)
                lifeImages[i].color = Color.white;
            else
                lifeImages[i].color = Color.clear;
        }
    }
}
