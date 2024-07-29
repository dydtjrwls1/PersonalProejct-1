using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    PlayerBase player;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = player.transform.position + Vector3.forward * -10.0f; 
    }
}
