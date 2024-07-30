using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Transform[] Slots;

    Transform[] TopSlots = new Transform[3];
    Transform[] BottomSlots = new Transform[3];
    Transform[] LeftSlots = new Transform[3];
    Transform[] RightSlots = new Transform[3];

    private void Awake()
    {
        Slots = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Slots[i] = transform.GetChild(i);
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BatchSlots()
    {
        foreach (var slot in Slots)
        {

        }
    }
}
