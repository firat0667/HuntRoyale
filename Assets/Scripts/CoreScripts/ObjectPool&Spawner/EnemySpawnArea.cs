using CoreScripts.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiratGames.CampSimulator.Event;

[System.Serializable]
public class EnemySpawnEntry
{
    public ComponentPool<Enemy> Pool;
    public int Weight = 1;
}

namespace CoreScripts.ObjectPool.Spawner
{
    public class EnemySpawnArea : MonoBehaviour
    {
        [Header("Spawn Area")]
        [SerializeField] private float m_radius = 10f;

        [Header("Spawn Rules")]
        [SerializeField] private int m_maxAliveEnemies = 5;
        [SerializeField] private Vector2 m_spawnIntervalRange = new Vector2(2f, 5f);

        [Header("Spawn Validation")]
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private LayerMask m_blockedLayers;
        [SerializeField] private float m_sphereCastRadius = 1f;
        [SerializeField] private float m_overlapRadius = 0.8f;
        [SerializeField] private float m_maxSlopeAngle = 45f;
        [SerializeField] private int m_maxSpawnTries = 25;

        [Header("Spawn Warning VFX")]
        [SerializeField] private EventKey m_spawnWarningVFXKey;
        [SerializeField] private float m_spawnDelay = 1.5f;

        [Header("Enemies That Can Spawn Here")]
        [SerializeField] private List<EnemySpawnEntry> m_enemies;

        private int m_totalWeight;
        private int m_aliveCount;

        private void Awake()
        {
            m_totalWeight = 0;
            foreach (var e in m_enemies)
                m_totalWeight += Mathf.Max(0, e.Weight);
        }

        private void OnEnable()
        {
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                float wait = Random.Range(
                    m_spawnIntervalRange.x,
                    m_spawnIntervalRange.y
                );

                yield return new WaitForSeconds(wait);
                TrySpawn();
            }
        }

        private void TrySpawn()
        {
            if (m_aliveCount >= m_maxAliveEnemies)
                return;

            EnemySpawnEntry entry = PickByWeight();
            if (entry == null || entry.Pool == null)
                return;

            if (!TryFindValidSpawnPoint(out Vector3 spawnPos))
                return;

            StartCoroutine(SpawnWithWarning(entry, spawnPos));
        }

        private IEnumerator SpawnWithWarning(
            EnemySpawnEntry entry,
            Vector3 spawnPos
        )
        {
            VFXManager.Instance.Play(
                m_spawnWarningVFXKey,
                spawnPos,
                Quaternion.identity,
                m_spawnDelay
            );

            yield return new WaitForSeconds(m_spawnDelay);

            Enemy enemy = entry.Pool.Retrieve();
            enemy.ResetForSpawn(entry.Pool);

            enemy.transform.SetPositionAndRotation(
                spawnPos,
                Quaternion.identity
            );

            m_aliveCount++;

            enemy.OnDeath.Disconnect(HandleEnemyDeath);
            enemy.OnDeath.Connect(HandleEnemyDeath);
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            enemy.OnDeath.Disconnect(HandleEnemyDeath);
            m_aliveCount = Mathf.Max(0, m_aliveCount - 1);
        }


        private bool TryFindValidSpawnPoint(out Vector3 spawnPoint)
        {
            spawnPoint = Vector3.zero;

            for (int i = 0; i < m_maxSpawnTries; i++)
            {
                Vector3 randomXZ = GetRandomPointXZ();

                if (!TryGetGroundSphere(randomXZ, out RaycastHit hit))
                    continue;

                if (!IsSlopeValid(hit))
                    continue;

                if (!IsAreaFree(hit.point))
                    continue;

                spawnPoint = hit.point;
                return true;
            }

            return false;
        }

        private bool TryGetGroundSphere(
            Vector3 randomXZ,
            out RaycastHit hit
        )
        {
            Vector3 origin = randomXZ + Vector3.up * 20f;

            return Physics.SphereCast(
                origin,
                m_sphereCastRadius,
                Vector3.down,
                out hit,
                50f,
                m_groundLayer
            );
        }

        private bool IsSlopeValid(RaycastHit hit)
        {
            float slope = Vector3.Angle(hit.normal, Vector3.up);
            return slope <= m_maxSlopeAngle;
        }

        private bool IsAreaFree(Vector3 point)
        {
            return !Physics.CheckSphere(
                point + Vector3.up * 0.5f,
                m_overlapRadius,
                m_blockedLayers
            );
        }

        private EnemySpawnEntry PickByWeight()
        {
            if (m_totalWeight <= 0)
                return null;

            int roll = Random.Range(0, m_totalWeight);
            int current = 0;

            foreach (var e in m_enemies)
            {
                current += e.Weight;
                if (roll < current)
                    return e;
            }

            return null;
        }

        private Vector3 GetRandomPointXZ()
        {
            Vector2 rnd = Random.insideUnitCircle * m_radius;
            return transform.position + new Vector3(rnd.x, 0f, rnd.y);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, m_radius);
        }
#endif
    }
}