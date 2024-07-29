using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : RecycleObject
{
    public float speed = 7.0f;

    float disableTime = 7.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        DIsableTimer(disableTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.up);
    }


}
