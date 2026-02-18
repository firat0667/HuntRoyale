using Firat0667.WesternRoyaleLib.Behavior.GameSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComponentPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T prefab;
    [SerializeField] private int initialSize = 10;

    private readonly Queue<T> _pool = new();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
            Create();
    }

    private T Create()
    {
        var obj = Instantiate(prefab, transform);
        obj.gameObject.SetActive(false);

        if (obj is IPool<T> poolable)
            poolable.Init(this);

        _pool.Enqueue(obj);
        return obj;
    }

    public T Retrieve()
    {
        if (_pool.Count == 0)
            Create();

        var obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}