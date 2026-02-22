using Firat0667.WesternRoyaleLib.Patterns;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using AI.Enemies;
using Firat0667.WesternRoyaleLib.Key;

namespace Managers.Enemies
{
    public class EnemyManager : FoundationSingleton<EnemyManager>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        private Dictionary<Transform, int> m_targetClaims = new();

        public List<Enemy> Enemies { get; private set; } = new();
        public BasicSignal<Enemy> EnemyDiedSignal { get; private set; } = new();
        public BasicSignal<Enemy> EnemySpawnedSignal { get; private set; } = new();


        private void OnEnable()
        {
            EnemyDiedSignal.Connect(UnregisterEnemy);
            EnemySpawnedSignal.Connect(RegisterEnemy);

        }
        private void OnDisable()
        {
            EnemyDiedSignal.Disconnect(UnregisterEnemy);
            EnemySpawnedSignal.Disconnect(RegisterEnemy);
        }
        public void RegisterEnemy(Enemy enemy)
        {
            if (!Enemies.Contains(enemy))
                Enemies.Add(enemy);
        }
        public void ClaimTarget(Transform target)
        {
            if (!m_targetClaims.ContainsKey(target))
                m_targetClaims[target] = 0;

            m_targetClaims[target]++;
        }
        public void ReleaseTarget(Transform target)
        {
            if (!m_targetClaims.ContainsKey(target)) return;

            m_targetClaims[target]--;

            if (m_targetClaims[target] <= 0)
                m_targetClaims.Remove(target);
        }
        public int GetClaimCount(Transform target)
        {
            return m_targetClaims.TryGetValue(target, out var count) ? count : 0;
        }
        public void UnregisterEnemy(Enemy enemy)
        {
            Enemies.Remove(enemy);
        }
    }
}

