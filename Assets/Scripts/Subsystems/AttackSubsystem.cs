using UnityEngine;

public class AttackSubsystem : Subsystem
{
    private AttackCore m_core;

    private float m_nextAttackTime;

    public bool IsInCombat { get; private set; }

    private CombatPerception m_perception;
    public Transform CurrentTarget => m_perception.CurrentTarget;

    protected override void Awake()
    {
        base.Awake();
        GetCoreComponent(ref m_core);
        m_perception = GetComponentInParent<CombatPerception>();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
       IsInCombat = m_perception.CurrentTarget != null &&
       m_perception.CurrentTargetSqrDistance <=
       StatsComponent.AttackRange * StatsComponent.AttackRange;
    }
    public bool TryAttack()
    {
        if (Time.time < m_nextAttackTime)
            return false;

        float attackRate = Mathf.Max(StatsComponent.AttackRate, 0.01f);
        float cooldown = 1f / attackRate;

        m_nextAttackTime = Time.time + cooldown;

        m_core.Attack(StatsComponent.AttackDamage);
        return true;
    }
}
