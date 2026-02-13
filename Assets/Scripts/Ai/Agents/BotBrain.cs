using Subsystems;
using UnityEngine;

public class BotBrain : MonoBehaviour
{
    [SerializeField] private BotAIBrainProfileSO m_profile;
    public Transform CurrentTarget { get; private set; }
    private StatsComponent m_stats;
    private HealthSubsystem m_health;
    private Transform m_healZone;
    public bool HasTarget => CurrentTarget != null;
    private void Awake()
    {
        m_stats = GetComponent<StatsComponent>();
        m_health = GetComponentInChildren<HealthSubsystem>();

        GameObject zone = GameObject.FindGameObjectWithTag(Tags.HealZone_Tag);
        if (zone != null)
            m_healZone = zone.transform;
    }
    public Vector3 DirectionToHealZone
    {
        get
        {
            if (m_healZone == null) return Vector3.zero;

            Vector3 dir = m_healZone.position - transform.position;
            dir.y = 0;
            return dir.normalized;
        }
    }

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
    public bool InAttackRange
    {
        get
        {
            if (!HasTarget) return false;
            return Vector3.Distance(
                transform.position,
                CurrentTarget.position
            ) <= m_stats.AttackStartRange;
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

    private void Update()
    {
        if(CurrentTarget != null)
            return;
        SelectTarget();
    }
    private void SelectTarget()
    {
        // we should find all enemies in the scene but on a list from a manager would be better
        var targets = GameObject.FindGameObjectsWithTag(Tags.Enemy_Tag);

        float bestScore = float.MinValue;
        Transform best = null;

        var myHealth = GetComponentInChildren<HealthSubsystem>();
        float myHpRatio = myHealth != null
            ? myHealth.CurrentHP / myHealth.MaxHP
            : 1f;

        foreach (var t in targets)
        {
            if (t.transform == transform) continue;

            var targetHealth = t.GetComponentInChildren<HealthSubsystem>();
            if (targetHealth == null) continue;

            float distance = Vector3.Distance(transform.position, t.transform.position);
            if (distance > m_profile.viewRadius) continue;

            float score = CalculateScore(
                t.transform,
                targetHealth,
                distance,
                myHpRatio
            );

            if (score > bestScore)
            {
                bestScore = score;
                best = t.transform;
            }
        }

        CurrentTarget = best;
    }

    private float CalculateScore(
    Transform target,
    HealthSubsystem targetHealth,
    float distance,
    float myHpRatio
)
    {
        float distanceScore = 1f - (distance / m_profile.viewRadius);
        float targetHpScore = 1f - targetHealth.CurrentHP / targetHealth.MaxHP;

        switch (m_profile.targetingMode)
        {
            case BotTargetingMode.ClosestOnly:
                return distanceScore;

            case BotTargetingMode.ClosestLowHP:
                return distanceScore * 0.4f + targetHpScore * 0.6f;

            case BotTargetingMode.SmartScore:
                {
                    float riskPenalty = myHpRatio < 0.3f ? 1f : 0f;

                    return
                        distanceScore * m_profile.awareness +
                        targetHpScore * m_profile.intelligence -
                        riskPenalty * m_profile.caution;
                }
        }

        return 0f;
    }

    public void ClearTarget()
    {
        CurrentTarget = null;
    }
}
