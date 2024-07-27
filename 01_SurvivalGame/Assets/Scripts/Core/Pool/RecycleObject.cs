using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleObject : MonoBehaviour
{
    public Action onDisable = null;

    protected virtual void OnEnable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        StopAllCoroutines();
    }

    protected virtual void OnDisable()
    {
        onDisable?.Invoke();
    }

    protected void DIsableTimer(float time = 0.0f)
    {
        StartCoroutine(DIsableAfterTime(time));
    }

    IEnumerator DIsableAfterTime(float time = 0.0f)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
