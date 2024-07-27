using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : RecycleObject 
{
    public GameObject prefab;

    public int poolSIze = 64;

    T[] pool;

    Queue<T> queue;
    
    public virtual void Initialize()
    {
        if (pool == null)
        {
            pool = new T[poolSIze];
            queue = new Queue<T>(poolSIze);

            GenerateObjectPool(0, poolSIze, pool);
        } else
        {
            foreach (T t in pool)
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    void GenerateObjectPool(int startIndex, int endIndex, T[] result)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.name = $"{prefab.name}_{i}";

            T comp = obj.GetComponent<T>();
            comp.onDisable += () => { queue.Enqueue(comp); };

            result[i] = comp;
            obj.SetActive(false);
        }
    }

    public T GetObject(Vector3? position = null, Vector3? eulerAngle = null)
    {
        if (queue.Count > 0)
        {
            T comp = queue.Dequeue();
            comp.gameObject.SetActive(true);
            comp.transform.position = position.GetValueOrDefault();
            comp.transform.rotation = Quaternion.Euler(eulerAngle.GetValueOrDefault());

            return comp;
        }
        else
        {
            int newSize = poolSIze * 2;
            T[] newPool = new T[newSize];

            Debug.LogWarning($"풀 사이즈 증가 {poolSIze} => {newSize}");

            for (int i = 0; i < poolSIze; i++)
                newPool[i] = pool[i];

            GenerateObjectPool(poolSIze, newSize, newPool);

            pool = newPool;
            poolSIze = newSize;

            return GetObject(position, eulerAngle);
        }
    }

}
