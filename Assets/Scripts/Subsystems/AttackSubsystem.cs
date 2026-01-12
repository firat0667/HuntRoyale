using UnityEngine;

public class AttackSubsystem : Subsystem
{
    private AttackCore m_core;
    private float m_lastAttackTime;

    protected override void Awake()
    {
        base.Awake();
        GetCoreComponent(ref m_core);
    }
    public void TryAttack()
    {
        float cooldown = 1f / StatsComponent.AttackRate;
        if (Time.time < m_lastAttackTime + cooldown)
            return;

        m_lastAttackTime = Time.time;
        m_core.Attack(StatsComponent.AttackDamage);
    }
}
