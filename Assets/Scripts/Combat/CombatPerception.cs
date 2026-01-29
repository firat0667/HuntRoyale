using UnityEngine;

public class CombatPerception : MonoBehaviour
{
    [SerializeField] private LayerMask m_targetLayer;
    [SerializeField] private float m_scanInterval = 0.15f;

    private StatsComponent m_stats;
    private float m_scanTimer;

    private readonly Collider[] m_hitsBuffer = new Collider[16];

    public Transform CurrentTarget { get; private set; }
    public float CurrentTargetSqrDistance { get; private set; }
    public LayerMask TargetLayer => m_targetLayer;

    private void Awake()
    {
        m_stats = GetComponent<StatsComponent>();
        m_scanTimer = Random.Range(0f, m_scanInterval);
    }

    private void Update()
    {
        m_scanTimer -= Time.deltaTime;
        if (m_scanTimer <= 0f)
        {
            m_scanTimer = m_scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        CurrentTarget = null;
        CurrentTargetSqrDistance = float.MaxValue;

        float maxScanRange = Mathf.Max(m_stats.AttackStartRange, m_stats.DetectionRange);

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            maxScanRange,
            m_hitsBuffer,
            m_targetLayer
        );

        for (int i = 0; i < hitCount; i++)
        {
            var hit = m_hitsBuffer[i];
            if (!hit) continue;

            float sqrDist =
                (hit.transform.position - transform.position).sqrMagnitude;

            if (sqrDist < CurrentTargetSqrDistance)
            {
                CurrentTargetSqrDistance = sqrDist;
                CurrentTarget = hit.transform;
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (m_stats == null)
            m_stats = GetComponent<StatsComponent>();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_stats.DetectionRange);

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
