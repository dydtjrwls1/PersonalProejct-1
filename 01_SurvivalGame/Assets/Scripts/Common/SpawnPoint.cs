using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float maxX = 13.0f;
    public float minX = -13.0f;

    public float maxY = 7.0f;
    public float minY = -7.0f;

    public Vector3 GetSpawnPoint()
    {
        return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

}
