using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T: MonoBehaviour
{
    private T prefab;
    private List<T> pool = new();
    public bool autoExpand = true;

    public ObjectPooler(T prefab, bool autoExpand = true)
    {
        this.prefab = prefab;
        this.autoExpand = autoExpand;
    }

    public void Pool(T objectToPool)
    {
        objectToPool.gameObject.SetActive(false);
        pool.Add(objectToPool);
    }

    public void Expand(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject expansion = Object.Instantiate(prefab.gameObject);
            Pool(expansion.GetComponent<T>());
        }
    }

    public void Despawn()
    {
        pool.Clear();
    }

    public T Unpool()
    {
        if (pool.Count <= 0)
        {
            if (autoExpand)
            {
                Expand();
                return Unpool();
            }
            else
            {
                Debug.LogError("Out of stock");
                return null;
            }
        }
        else
        {
            T pooled = pool[0];
            pool.Remove(pooled);
            pooled.gameObject.SetActive(true);
            return pooled;
        }
    }
}
