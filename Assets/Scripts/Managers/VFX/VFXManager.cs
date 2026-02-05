using CoreScripts.ObjectPool;
using Firat0667.CaseLib.Key;
using Firat0667.CaseLib.Patterns;
using FiratGames.CampSimulator.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : FoundationSingleton<VFXManager>, IFoundationSingleton
{
    public bool Initialized { get ; set ; }

    [SerializeField] private List<VFXEntry> m_vfxEntries = new();
    private Dictionary<string, GameObject> m_prefabs = new();

    [SerializeField] private int m_preloadCount = 10;
    [SerializeField] private VFXPool m_vfxPool;

    [System.Serializable]
    public class VFXEntry
    {
        public EventKey key;        
        public GameObject prefab;  
    }


    private void Start()
    {
        if (m_vfxPool == null)
        {
            Debug.LogError("[VFXManager] VFXPool not found!");
            return;
        }

        foreach (var entry in m_vfxEntries)
        {
            if (entry.prefab == null || entry.key == null)
                continue;

            var gameKey = entry.key.StringKey;

            if (!m_prefabs.ContainsKey(gameKey))
                m_prefabs.Add(gameKey, entry.prefab);
        }
    }
    public GameObject Play(
        EventKey key,
        Vector3 position,
        Quaternion rotation,
        float autoReturnTime = 2f,
        Transform parent = null
    )
    {
        if (!m_prefabs.TryGetValue(key.Key.ValueAsString, out var prefab))
        {
            Debug.LogWarning($"[VFXManager] VFX key not found: {key.Key?.ValueAsString}");
            return null;
        }

        var vfx = m_vfxPool.Retrieve();
        vfx.transform.SetPositionAndRotation(position, rotation);

        if (parent != null)
            vfx.transform.SetParent(parent);

        if (autoReturnTime > 0f)
            StartCoroutine(ReturnAfterDelay(vfx, autoReturnTime));

        return vfx.gameObject;
    }
    private IEnumerator ReturnAfterDelay(VFXObject vfx, float time)
    {
        yield return new WaitForSeconds(time);

        if (m_vfxPool != null)
            m_vfxPool.Return(vfx);
        else
            vfx.gameObject.SetActive(false);
    }
}
