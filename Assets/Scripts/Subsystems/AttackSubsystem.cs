using UnityEngine;

public class AttackSubsystem : Subsystem
{
    private AttackCore m_core;
    private StatsComponent m_stats;
    private float m_lastAttackTime;
    void Start()
    {
        GetCoreComponent(ref m_core);
        m_stats = GetComponentInParent<StatsComponent>();
    }

    public void TryAttack()
    {
        float cooldown = 1f / m_stats.AttackRate;
        if (Time.time < m_lastAttackTime + cooldown)
            return;

        m_lastAttackTime = Time.time;
        m_core.Attack(m_stats.AttackDamage);
    }
}
