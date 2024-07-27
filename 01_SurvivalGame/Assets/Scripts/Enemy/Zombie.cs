using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : RecycleObject
{
    float speed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.right);
    }
}
