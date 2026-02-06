using CoreScripts.ObjectPool;
using Firat0667.CaseLib.Key;
using Firat0667.CaseLib.Patterns;
using FiratGames.CampSimulator.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : FoundationSingleton<VFXManager>, IFoundationSingleton
{
    public bool Initialized { get; set; }

    [SerializeField] private List<VFXEntry> m_vfxEntries;
    [SerializeField] private int m_preloadCount = 10;
    [SerializeField] private VFXPool m_vfxPool;

    private Dictionary<string, GameObject> m_prefabs = new();

    [System.Serializable]
    public class VFXEntry
    {
        public EventKey key;
        public GameObject prefab;
    }

    private void Start()
    {
        foreach (var entry in m_vfxEntries)
        {
            if (entry.key == null || entry.prefab == null)
                continue;

            var k = entry.key.StringKey;
            m_prefabs[k] = entry.prefab;
            m_vfxPool.Preload(entry.prefab, m_preloadCount);
        }
    }
    public GameObject Play(
        EventKey key,
        Vector3 pos,
        Quaternion rot,
        float autoReturnTime = 2f,
        Transform parent = null)
    {
        if (!m_prefabs.TryGetValue(key.StringKey, out var prefab))
            return null;

        var go = m_vfxPool.Retrieve(prefab, pos, rot, parent);

        if (autoReturnTime > 0)
            StartCoroutine(ReturnAfterDelay(prefab, go, autoReturnTime));

        return go;
    }

    private IEnumerator ReturnAfterDelay(GameObject prefab, GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
        m_vfxPool.ReturnToPool(prefab, go);
    }
}
