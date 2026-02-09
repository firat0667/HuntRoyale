using UnityEngine;

namespace Combat {
    public class CombatPerception : MonoBehaviour
    {
        [SerializeField] private LayerMask m_targetLayer;
        [SerializeField] private LayerMask m_obstacleLayer;
        [SerializeField] private float m_scanInterval = 0.15f;
        [SerializeField] private float m_followInterval = 3.0f;

        private StatsComponent m_stats;
        private float m_scanTimer;
        private float m_followTimer;
       
        private float m_currentDetectionRange;
        private float m_defaultDetectionRange => m_stats.DetectionRange;

        private readonly Collider[] m_hitsBuffer = new Collider[16];

        public Transform CurrentTarget { get; private set; }
        public float CurrentTargetSqrDistance { get; private set; }
        public LayerMask TargetLayer => m_targetLayer;
        public float CurrentDetectionRange => m_currentDetectionRange;
     

        private void Awake()
        {
            m_stats = GetComponent<StatsComponent>();
            m_scanTimer = Random.Range(0f, m_scanInterval);
            m_followTimer = m_followInterval;
            m_currentDetectionRange = m_defaultDetectionRange;

        }

        private void Update()
        {
            m_followTimer -= Time.deltaTime;
            if (m_followTimer < 0f)
            {
                m_currentDetectionRange = m_defaultDetectionRange;
            }

            m_scanTimer -= Time.deltaTime;
            if (m_scanTimer <= 0f)
            {
                m_scanTimer = m_scanInterval;
                Scan();
            }
        }
        public void SetCurrentTarget(Transform target)
        {
            CurrentTarget = target;
            m_followTimer = m_followInterval;
            float distance = Vector3.Distance(target.position,this.transform.position);
            m_currentDetectionRange = Mathf.Max(m_defaultDetectionRange, distance * 2f);
        }
        public void ClearTarget()
        {
            CurrentTarget = null;
             CurrentTargetSqrDistance = float.MaxValue;
            m_currentDetectionRange = m_defaultDetectionRange;
        }

        private void Scan()
        {
            Transform bestTarget = CurrentTarget;
            float bestSqr = bestTarget != null
                ? (bestTarget.position - transform.position).sqrMagnitude
                : float.MaxValue;

            float scanRange = Mathf.Max(m_stats.AttackStartRange, m_currentDetectionRange);

            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                scanRange,
                m_hitsBuffer,
                m_targetLayer
            );

            for (int i = 0; i < hitCount; i++)
            {
                var hit = m_hitsBuffer[i];
                if (!hit) continue;
                float sqrDist =
                    (hit.transform.position - transform.position).sqrMagnitude;

                if (sqrDist < bestSqr)
                {
                    bestSqr = sqrDist;
                    bestTarget = hit.transform;
                }
            }

            CurrentTarget = bestTarget;
            CurrentTargetSqrDistance = bestTarget != null
                ? bestSqr
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

