using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Object
{
    #region Private Fields
    private T prefab;
    private int initialSize = 10;
    private Queue<T> pool = new Queue<T>();
    #endregion
    #region Constructors
    public ObjectPool(T prefab, int inialSize = 5)
    {
        this.prefab = prefab;
        this.initialSize = inialSize;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab);
            pool.Enqueue(obj);
        }
    }
    #endregion
    #region Public Methods
    public T GetFromPool()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            return obj;
        }
        else
        {
            T obj = Object.Instantiate(prefab);
            return obj;
        }
    }

    public void ReturnToPool(T obj)
    {
        pool.Enqueue(obj);
    }
    #endregion
}