using Firat0667.WesternRoyaleLib.Key;
using Game;
using Subsystems;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.Timeline;

namespace Combat
{
    public class CombatPerception : MonoBehaviour
    {
        [SerializeField] private bool m_scanOnStart = true;

        [SerializeField] private LayerMask m_targetLayer;
        [SerializeField] private LayerMask m_obstacleLayer;


        [SerializeField] private float m_scanInterval = 0.15f;
        [SerializeField] private float m_followInterval = 3.0f;
        [SerializeField] private float m_aggroDuration = 1.5f;

        private StatsComponent m_stats;
        private float m_scanTimer;
        private float m_followTimer;

        private Transform m_recentAttacker;
        private float m_aggroTimer;
   
        private float m_currentDetectionRange;
        private float m_defaultDetectionRange => m_stats.DetectionRange;

        private readonly Collider[] m_hitsBuffer = new Collider[16];

        public Transform CurrentTarget { get; private set; }
        public float CurrentTargetSqrDistance { get; private set; }
        public LayerMask TargetLayer => m_targetLayer;
        public float CurrentDetectionRange => m_currentDetectionRange;

        public BasicSignal<Transform> OnTargetChanged { get; private set; } = new BasicSignal<Transform>();

        private void Awake()
        {
            m_stats = GetComponent<StatsComponent>();
            m_scanTimer = Random.Range(0f, m_scanInterval);
            m_followTimer = m_followInterval;
            m_currentDetectionRange = m_defaultDetectionRange;
            m_aggroTimer = 0;
        }
        public void OnDamaged(Transform attacker)
        {
            if(m_aggroTimer>0)
                return;
            m_recentAttacker = attacker;
            m_aggroTimer = m_aggroDuration;
        }
        private void Update()
        {
            m_followTimer -= Time.deltaTime;
            if (m_followTimer < 0f)
            {
                m_currentDetectionRange = m_defaultDetectionRange;
            }

            if (m_aggroTimer > 0f)
                m_aggroTimer -= Time.deltaTime;
            else
                m_recentAttacker = null;

            m_scanTimer -= Time.deltaTime;
            if (m_scanTimer <= 0f)
            {
                m_scanTimer = m_scanInterval;
                Scan();
            }
        }
        public void SetCurrentTarget(Transform target)
        {
            if (target == null)
            {
                ClearTarget();
                return;
            }
            if (CurrentTarget == target)
            {
                m_followTimer = m_followInterval;
                return;
            }

            CurrentTarget = target;
            m_followTimer = m_followInterval;
            OnTargetChanged.Emit(CurrentTarget);
            float distance = Vector3.Distance(target.position, transform.position);
            m_currentDetectionRange = Mathf.Max(m_defaultDetectionRange, distance * 2f);
        }
        public void ClearTarget()
        {
            if (CurrentTarget == null)
                return;

            CurrentTarget = null;

            OnTargetChanged.Emit(null);

            CurrentTargetSqrDistance = float.MaxValue;
            m_currentDetectionRange = m_defaultDetectionRange;
        }

        private void Scan()
        {
            if (m_recentAttacker != null)
            {
                SetCurrentTarget(m_recentAttacker);
                CurrentTargetSqrDistance =
                    (m_recentAttacker.position - transform.position).sqrMagnitude;

                return;
            }
            float scanRange = Mathf.Max(m_stats.EffectiveAttackRange, m_currentDetectionRange);
            float scanRangeSqr = scanRange * scanRange;

            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                scanRange,
                m_hitsBuffer,
                m_targetLayer
            );

            Transform bestTarget = null;
            float bestScore = float.MinValue;

            for (int i = 0; i < hitCount; i++)
            {
                var hit = m_hitsBuffer[i];
                if (hit.transform == transform)
                    continue;
                if (!hit) continue;
                if(!hit.gameObject.activeInHierarchy) continue;
                Vector3 toEnemy = hit.transform.position - transform.position;
                float sqrDist = toEnemy.sqrMagnitude;

                if (sqrDist > scanRangeSqr)
                    continue;

                float distanceScore = -sqrDist;

                float dot = Vector3.Dot(transform.forward, toEnemy.normalized);
                float frontScore = dot * 5f;
                float inAttackRangeBonus =
                    sqrDist <= m_stats.AttackStartRange * m_stats.AttackStartRange
                    ? 10f
                    : 0f;

                float totalScore = distanceScore + frontScore + inAttackRangeBonus;

                if (hit.transform == CurrentTarget)
                    totalScore += 15f;

                if (totalScore > bestScore)
                {
                    bestScore = totalScore;
                    bestTarget = hit.transform;
                }
            }
            // TODO: Change it later to make it more robust. For example,
            // if the recent attacker is still valid but not the best target,
            // it should still be the current target instead of switching to the best target.
  
            if (m_scanOnStart)
            {
                if (bestTarget != null && bestTarget != CurrentTarget)
                {
                    CurrentTarget = bestTarget;
                    OnTargetChanged.Emit(CurrentTarget);
                }
                else
                {
                    CurrentTarget = bestTarget;
                }
            }
            CurrentTargetSqrDistance = bestTarget != null
             ? (bestTarget.position - transform.position).sqrMagnitude
             : float.MaxValue;
        }
        public bool HasClearLineOfSight(Transform target)
        {
            if (target == null)
                return false;

            Vector3 origin = transform.position;
            Vector3 targetPos = target.position;

            origin.y = 0.5f;
            targetPos.y = 0.5f;

            Vector3 dir = targetPos - origin;
            float dist = dir.magnitude;

            if (Physics.Raycast(
                origin,
                dir.normalized,
                out RaycastHit hit,
                dist,
                m_obstacleLayer
            ))
            {
                return false;
            }

            return true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_stats == null)
                m_stats = GetComponent<StatsComponent>();

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_currentDetectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_stats.AttackStartRange);

            if (CurrentTarget != null)
            {
                bool inAttackRange =
                    CurrentTargetSqrDistance <= m_stats.AttackStartRange * m_stats.AttackStartRange;

                Gizmos.color = inAttackRange ? Color.red : Color.cyan;

                Gizmos.DrawLine(transform.position, CurrentTarget.position);
                Gizmos.DrawSphere(CurrentTarget.position, 0.15f);
            }
        }
#endif
    }
}

