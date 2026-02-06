using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    private Dictionary<GameObject, Queue<GameObject>> _pools = new();

    public void Preload(GameObject prefab, int count)
    {
        if (!_pools.ContainsKey(prefab))
            _pools[prefab] = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(prefab, transform);
            go.SetActive(false);
            _pools[prefab].Enqueue(go);
        }
    }

    public GameObject Retrieve(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        if (!_pools.ContainsKey(prefab))
            _pools[prefab] = new Queue<GameObject>();

        GameObject go = _pools[prefab].Count > 0
            ? _pools[prefab].Dequeue()
            : Instantiate(prefab);

        go.transform.SetParent(parent ? parent : transform);
        go.transform.SetPositionAndRotation(pos, rot);
        go.transform.localScale = Vector3.one;

        var ps = go.GetComponent<ParticleSystem>();
        if (ps)
        {
            ps.Clear(true);
            ps.Play(true);
        }

        go.SetActive(true);
        return go;
    }

    public void ReturnToPool(GameObject prefab, GameObject go)
    {
        if (!_pools.ContainsKey(prefab))
            _pools[prefab] = new Queue<GameObject>();

        go.SetActive(false);
        go.transform.SetParent(transform);
        _pools[prefab].Enqueue(go);
    }
}