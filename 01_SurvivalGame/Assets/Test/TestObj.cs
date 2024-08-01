using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : MonoBehaviour
{
    public Transform target;

    float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > 5.0f)
        {
            Factory.Instance.GetStrongZombie(target.transform.position);
            elapsedTime = 0f;
        }
    }
}
