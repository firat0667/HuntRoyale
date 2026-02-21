using AI.Agents;
using AI.ScriptableObjects;
using Game;
using Subsystems;
using UnityEngine;

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

        public Transform CurrentTarget { get; private set; }
        public bool HasTarget => CurrentTarget != null;

        private void Awake()
        {
            m_stats = GetComponent<StatsComponent>();
            m_health = GetComponentInChildren<HealthSubsystem>();
            m_agent = GetComponent<Agent>();

            GameObject zone = GameObject.FindGameObjectWithTag(Tags.HealZone_Tag);
            if (zone != null)
                m_healZone = zone.transform;

            m_scanTimer = Random.Range(0f, m_scanInterval);
        }

        private void Update()
        {
            m_scanTimer -= Time.deltaTime;

            if (CurrentTarget != null)
            {
                if (!IsTargetValid(CurrentTarget))
                    CurrentTarget = null;
                else
                    return;
            }

            if (m_scanTimer <= 0f)
            {
                m_scanTimer = m_scanInterval;
                SelectTarget();
            }
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

        public Vector3 DirectionToTarget
        {
            get
            {
                if (!HasTarget) return Vector3.zero;

                Vector3 dir = CurrentTarget.position - transform.position;
                dir.y = 0;
                return dir.normalized;
            }
        }

        #endregion

        #region Target
        public void ClearTarget()
        {
            CurrentTarget = null;
        }
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
            var targets = GameObject.FindGameObjectsWithTag(Tags.Enemy_Tag);

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

                float score = CalculateScore(t.transform, targetHealth, distance, myHpRatio);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = t.transform;
                }
            }

            CurrentTarget = best;
            if(CurrentTarget!=null)
            m_agent.CombatPerception.SetCurrentTarget(CurrentTarget);
        }
        public void SetProfile(BotAIBrainProfileSO profile)
        {
            m_profile = profile;
        }
        private float CalculateScore(
            Transform target,
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
                    return
                        distanceScore * m_profile.awareness +
                        targetHpScore *
                        (m_profile.intelligence +
                         m_profile.aggressiveness +
                         m_profile.greed) -
                        riskPenalty * m_profile.caution;
            }

            return 0f;
        }

        #endregion
    }
}
