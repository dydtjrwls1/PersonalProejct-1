using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector3 triggeredSlotPos;

    Vector3 exitPos;

    Slot[] slots;

    const float LENGTH = 28.0f;

    private void Awake()
    {
        slots = GetComponentsInChildren<Slot>();
    }

    public void MoveSlots()
    {
        float diffX = Mathf.Abs(exitPos.x - triggeredSlotPos.x);
        float diffY = Mathf.Abs(exitPos.y - triggeredSlotPos.y);

        if (diffX > LENGTH / 2.0f)
        {
            if (exitPos.x > triggeredSlotPos.x)
            {
                foreach (var slot in slots)
                {
                    if (slot.transform.position.x < triggeredSlotPos.x)
                    {
                        slot.MoveRight();
                    }
                }
            }
            else
            {
                foreach (var slot in slots)
                {
                    if (slot.transform.position.x > triggeredSlotPos.x)
                    {
                        slot.MoveLeft();
                    }
                }
            }
        } 

        if (diffY > LENGTH / 2.0f)
        {
            if (exitPos.y > triggeredSlotPos.y)
            {
                foreach (var slot in slots)
                {
                    if (slot.transform.position.y < triggeredSlotPos.y)
                    {
                        slot.MoveTop();
                    }
                }
            } else
            {
                foreach (var slot in slots)
                {
                    if (slot.transform.position.y > triggeredSlotPos.y)
                    {
                        slot.MoveBottom();
                    }
                }
            }
        }
    }

    /// <summary>
    /// ���� Exit �� �߻��� ������ Position �� Ż���� �÷��̾��� ��ġ�� �޴� �Լ�
    /// </summary>
    /// <param name="collisionPos">collision �� Ż���� ��ġ��ǥ</param>
    /// <param name="slotPosition">exit �� �߻��� ������ ��ġ��ǥ</param>
    public void SetTriggerPosition(Vector3 collisionPos, Vector3 slotPosition)
    {
        exitPos = collisionPos;
        triggeredSlotPos = slotPosition;  
    }
}
