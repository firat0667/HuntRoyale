using Combat;
using CoreScripts.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SummonPoolEntry
{
    public int Id;
    public ComponentPool<SummonMinion> Pool;
}
namespace Firat0667.WesternRoyaleLib.Game
{
    public class SummonSpawner : MonoBehaviour
    {
        [SerializeField] private List<SummonPoolEntry> m_summonPools;
        private Dictionary<int, ComponentPool<SummonMinion>> _poolMap;
        private void Awake()
        {
            _poolMap = new Dictionary<int, ComponentPool<SummonMinion>>();

            foreach (var entry in m_summonPools)
            {
                if (entry.Pool == null)
                    continue;

                _poolMap[entry.Id] = entry.Pool;
            }
        }
        private void Start()
        {
            GameRegistry.Instance.Register(GameRegistryTags.GAME_REGISTRY_SPAWNER_SUMMON, this, true);
        }
        private void OnDisable()
        {
            GameRegistry.Instance.Unregister(GameRegistryTags.GAME_REGISTRY_SPAWNER_SUMMON);
        }
        public void Spawn(
            int summonId,
            Vector3 position,
            float damage,
            Transform target,
            Transform owner,
            float explosionRadius,
            float explosionTriggerDistance,
            LayerMask targetLayer
        )
        {
            if (!_poolMap.TryGetValue(summonId, out var pool))
            {
                Debug.LogError($"SummonSpawner: Pool not found for ID {summonId}");
                return;
            }

            SummonMinion summon = pool.Retrieve();
            summon.transform.position = position;

            summon.Init(
                damage,
                target,
                owner,
                explosionRadius,
                explosionTriggerDistance,
                targetLayer,
                pool
            );
        }


    }
}

