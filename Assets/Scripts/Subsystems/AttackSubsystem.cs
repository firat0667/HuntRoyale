using UnityEngine;

public class AttackSubsystem : Subsystem
{
    private AttackCore m_core;

    private float _nextAttackTime;

    protected override void Awake()
    {
        base.Awake();
        GetCoreComponent(ref m_core);
    }

    public bool TryAttack()
    {
        if (Time.time < _nextAttackTime)
            return false;

        float attackRate = Mathf.Max(StatsComponent.AttackRate, 0.01f);
        float cooldown = 1f / attackRate;

        _nextAttackTime = Time.time + cooldown;

        m_core.Attack(StatsComponent.AttackDamage);
        return true;
    }
}
