using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    const float LENGTH = 28.0f;

    Background background;

    private void Awake()
    {
        background = GetComponentInParent<Background>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        background.SetTriggerPosition(collision.transform.position, transform.position);
        background.MoveSlots();
    }

    public void MoveRight()
    {
        transform.position += 3.0f * LENGTH * Vector3.right;
    }

    public void MoveLeft()
    {
        transform.position += 3.0f * LENGTH * Vector3.left;
    }

    public void MoveTop()
    {
        transform.position += 3.0f * LENGTH * Vector3.up;
    }

    public void MoveBottom()
    {
        transform.position += 3.0f * LENGTH * Vector3.down;
    }
}
