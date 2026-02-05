
using Firat0667.CaseLib.Pattern.Pool;
using Firat0667.CaseLib.Diagnostics;
using UnityEngine;

namespace Firat0667.CaseLib.Game
{
    /// <summary>
    /// Spawner with extra batch spawning.
    /// </summary>
    public abstract class BatchSpawner : Spawner
    {
        public void BatchSpawn(int batchCount)
        {
            for (int i = 0; i < batchCount; i++)
            {
                Spawn();
            }
        }
    }

    /// <summary>
    /// Common spawner functionality.
    /// </summary>
    public abstract class Spawner : MonoBehaviour
    {
        [SerializeField] protected GameObjectPool _spawnPool;
        [SerializeField] protected Transform _spawnTransform;

        private protected ManualStopwatch _unityStopwatch = new();

        protected GameObject Spawn(Transform spawnpos=null)
        {
            var go = _spawnPool.Retrieve();
            go.transform.SetPositionAndRotation(_spawnTransform.position, _spawnTransform.rotation);
            return go;
        }
    }
    public abstract class BaseSpawner : MonoBehaviour
    {
        protected GameObject SpawnFromPool(
            GameObjectPool pool,
            Transform spawnPoint
        )
        {
            if (pool == null)
            {
                Debug.LogError($"{GetType().Name}: Pool is null!");
                return null;
            }

            if (spawnPoint == null)
            {
                Debug.LogError($"{GetType().Name}: SpawnPoint is null!");
                return null;
            }

            var go = pool.Retrieve();
            go.transform.SetPositionAndRotation(
                spawnPoint.position,
                spawnPoint.rotation
            );
            return go;
        }
    }
}