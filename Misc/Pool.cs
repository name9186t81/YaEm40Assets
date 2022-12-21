using System;
using System.Collections.Generic;

public class Pool<T>
{
    private Queue<T> _pooled = new Queue<T>();
    private Func<T> _createObjectFunc;
    public Pool(Func<T> createObject)
    {
        _createObjectFunc = createObject;
    }
    public T GetFromPool()
    {
        if (_pooled.Count == 0)
        {
            return _createObjectFunc();
        }
        return _pooled.Dequeue();
    }
    public void ReturnToPool(T obj)
    {
        _pooled.Enqueue(obj);
    }
}
