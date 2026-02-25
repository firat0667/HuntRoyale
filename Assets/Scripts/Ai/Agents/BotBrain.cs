using AI.Agents;
using AI.ScriptableObjects;
using Game;
using Subsystems;
using UnityEngine;
using Managers.Enemies;
using Managers.Game;
using AI.Enemies;

namespace AI.Brain 
{
    public class BotBrain : MonoBehaviour
    {
        [SerializeField] private BotAIBrainProfileSO m_profile;
        [SerializeField] private float m_scanInterval = 0.3f;

        private StatsComponent m_stats;
        private HealthSubsystem m_health;
        private Transform m_healZone;

        private float m_scanTimer;
        private Agent m_agent;
        
        public Transform CurrentTarget=> m_agent.CombatPerception.CurrentTarget;
        public bool HasTarget => m_agent.CombatPerception.CurrentTarget != null;

        private void Awake()
        {
            m_stats = GetComponent<StatsComponent>();
            m_health = GetComponentInChildren<HealthSubsystem>();
            m_agent = GetComponent<Agent>();

                m_healZone = GameManager.Instance.HealZone;

            m_scanTimer = Random.Range(0f, m_scanInterval);
        }

        private void Update()
        {
            m_scanTimer -= Time.deltaTime;

            if (HasTarget && !IsTargetValid(CurrentTarget))
            {
                ClearTargetAndPerception();
                m_scanTimer = 0f; 
            }
            if (!HasTarget && m_scanTimer <= 0f)
            {
                m_scanTimer = m_scanInterval;
                SelectTarget();
            }
        }
        private void ClearTargetAndPerception()
        {

            if (m_agent != null && m_agent.CombatPerception != null)
                m_agent.CombatPerception.SetCurrentTarget(null);
        }
        #region Heal

        public bool ShouldHeal
        {
            get
            {
                if (m_health == null) return false;

                float hpRatio = (float)m_health.CurrentHP / m_health.MaxHP;

                float healThreshold =
                    0.4f
                    - m_profile.aggressiveness * 0.25f
                    + m_profile.caution * 0.35f;

                return hpRatio <= healThreshold;
            }
        }

        public Transform GetHealZone()
        {
            return m_healZone;
        }

        #endregion

        #region Combat

        public bool InAttackRange
        {
            get
            {
                if (!HasTarget) return false;

                float range = m_stats.EffectiveAttackRange;
                return (CurrentTarget.position - transform.position).sqrMagnitude
                       <= range * range;
            }
        }


        #endregion

        #region Target
        private bool IsTargetValid(Transform target)
        {
            if (target == null) return false;

            float sqrDist = (target.position - transform.position).sqrMagnitude;

            if (sqrDist > m_stats.DetectionRange * m_stats.DetectionRange)
                return false;

            var hp = target.GetComponentInChildren<HealthSubsystem>();
            if (hp == null || hp.IsDead)
                return false;

            return true;
        }

        private void SelectTarget()
        {
            var targets = EnemyManager.Instance.Enemies;
           
            float bestScore = float.MinValue;
            Transform best = null;

            float myHpRatio = (float)m_health.CurrentHP / m_health.MaxHP;

            foreach (var t in targets)
            {
                if (t.transform == transform) continue;

                var targetHealth = t.GetComponentInChildren<HealthSubsystem>();
                if (targetHealth == null || targetHealth.IsDead) continue;

                float distance = Vector3.Distance(transform.position, t.transform.position);
                if (distance > m_stats.DetectionRange) continue;

                float score = CalculateScore(t, targetHealth, distance, myHpRatio);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = t.transform;
                }
            }
            Transform oldTarget = CurrentTarget;

            if (oldTarget != null)
            {
                EnemyManager.Instance.ReleaseTarget(oldTarget);
            }

            m_agent.CombatPerception.SetCurrentTarget(best);

            if (best != null)
            {
                EnemyManager.Instance.ClaimTarget(best);
            }

            m_agent.CombatPerception.SetCurrentTarget(best);
        }
        public void SetProfile(BotAIBrainProfileSO profile)
        {
            m_profile = profile;
        }
        private float CalculateScore(
            Enemy target,
            HealthSubsystem targetHealth,
            float distance,
            float myHpRatio)
        {
            float distanceScore = 1f - (distance / m_stats.DetectionRange);
            float targetHpScore = 1f - ((float)targetHealth.CurrentHP / targetHealth.MaxHP);
            float riskPenalty = myHpRatio < 0.3f ? 1f : 0f;

            switch (m_profile.targetingMode)
            {
                case BotTargetingMode.ClosestOnly:
                    return distanceScore;

                case BotTargetingMode.ClosestLowHP:
                    return distanceScore * 0.4f + targetHpScore * 0.6f;

                case BotTargetingMode.SmartScore:

                    float baseScore =
                        distanceScore * m_profile.awareness +
                        targetHpScore *
                        (m_profile.intelligence +
                         m_profile.aggressiveness) -
                        riskPenalty * m_profile.caution;

                    float rewardWeight = m_profile.greed;

                    float rewardScore = 0f;

                    if (target.KillRewards != null)
                    {
                        var rewards = target.KillRewards;

                        rewardScore += rewards.expReward * 0.02f;
                        rewardScore += rewards.scoreReward * 0.01f;

                        float starChanceTotal = 0f;
                        foreach (var entry in rewards.killRewardEntries)
                            starChanceTotal += entry.spawnChance;

                        rewardScore += starChanceTotal * 2f;
                    }

                    int claimCount = EnemyManager.Instance.GetClaimCount(target.transform);
                    float claimPenalty =
                        claimCount * (1f - m_profile.greed) * 0.4f;

                    return baseScore + rewardScore * rewardWeight - claimPenalty;
            }

            return 0f;
        }

        #endregion
    }
}
