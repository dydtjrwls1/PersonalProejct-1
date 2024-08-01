using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPoint : MonoBehaviour
{
    public float rotateSpeed = 120.0f;

    private void FixedUpdate()
    {
        transform.Rotate(Time.deltaTime * rotateSpeed * Vector3.forward);
    }
}
