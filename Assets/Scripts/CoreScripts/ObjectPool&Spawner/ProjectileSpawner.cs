using Combat;
using CoreScripts.ObjectPool;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class ProjectilePoolEntry
{
    public int Id;
    public ComponentPool<Projectile> Pool;
}

namespace Firat0667.CaseLib.Game
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private List<ProjectilePoolEntry> m_projectilePools;
        private Dictionary<int, ComponentPool<Projectile>> _poolMap;

        
        private void Awake()
        {
            _poolMap = new Dictionary<int, ComponentPool<Projectile>>();

            foreach (var entry in m_projectilePools)
            {
                if (entry.Pool == null)
                    continue;

                _poolMap[entry.Id] = entry.Pool;
            }
          
        }
        private void Start()
        {
            GameRegistry.Instance.Register(GameRegistryTags.GAME_REGISTRY_SPAWNER_PROJECTILE, this, true);
        }
        private void OnDisable()
        {
            GameRegistry.Instance.Unregister(GameRegistryTags.GAME_REGISTRY_SPAWNER_PROJECTILE);
        }
        public void Spawn(
            int projectileId,
            Transform spawnPoint,
            float damage,
            List<Transform> targets,
            Transform source,
            float speed
        )
        {
            if (!_poolMap.TryGetValue(projectileId, out var pool))
            {
                Debug.LogError($"ProjectileSpawner: Pool not found for ID {projectileId}");
                return;
            }

            Projectile projectile = pool.Retrieve();

            projectile.transform.position = spawnPoint.position;

            projectile.Init(damage, targets, source, speed,pool);
        }
    }
}