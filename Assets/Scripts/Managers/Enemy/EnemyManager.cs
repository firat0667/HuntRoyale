using Firat0667.WesternRoyaleLib.Patterns;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using AI.Enemies;
using Firat0667.WesternRoyaleLib.Key;
using Subsystems;
using Managers.Score;

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
            EnemyDiedSignal.Connect(OnEnemyDied);

        }
        private void OnDisable()
        {
            EnemyDiedSignal.Disconnect(UnregisterEnemy);
            EnemySpawnedSignal.Disconnect(RegisterEnemy);
            EnemyDiedSignal.Disconnect(OnEnemyDied);
        }
        public void RegisterEnemy(Enemy enemy)
        {
            if (!Enemies.Contains(enemy)) { }
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

        private void OnEnemyDied(Enemy enemy)
        {
            ProcessKillRewards(enemy);
        }
        private void ProcessKillRewards(Enemy enemy)
        {
            var health = enemy.GetSubsystem<HealthSubsystem>();
            if (health.LastDamageSource == null)
                return;

            var killer = health.LastDamageSource.GetComponentInParent<BaseEntity>();
            if (killer == null)
                return;

            var rewards = enemy.KillRewards;
            if (rewards == null)
                return;

            var exp = killer.GetSubsystem<ExperienceSubsystem>();
            if (exp != null)
            {
                var stats = killer.GetComponent<StatsComponent>();
                int modified = stats.ModifyExp(rewards.expReward);
                exp.AddExp(modified);
            }
            ScoreManager.Instance.AddScore(killer, rewards.scoreReward);

            //SpawnDrops(enemy.transform.position, rewards);
        }
    }
}

