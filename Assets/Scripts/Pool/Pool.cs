using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : Component
{
    private T prefab;
    private List<T> pool = new List<T>();
    private Transform parentContainer;

    public Pool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        parentContainer = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parentContainer);
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    public T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool[0];
            pool.RemoveAt(0);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T obj = GameObject.Instantiate(prefab, parentContainer);
            return obj;
        }
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Add(obj);
    }
}
