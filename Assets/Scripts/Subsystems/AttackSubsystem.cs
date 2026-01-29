using Subsystems.CoreComponents;
using UnityEngine;


namespace Subsystems
{
    public class AttackSubsystem : Subsystem
    {
        private AttackCore m_core;

        private float m_nextAttackTime;

        public bool IsTargetInAttackRange { get; private set; }

        private CombatPerception m_perception;
        public CombatPerception Perception => m_perception;
        public Transform CurrentTarget => m_perception.CurrentTarget;

        public float AttackStartRange => StatsComponent.AttackStartRange;
        public float AttackAngle => StatsComponent.AttackAngle;
        public float attackHitRange => StatsComponent.AttackHitRange;   
        public float DetectRange => StatsComponent.DetectionRange;

        protected override void Awake()
        {
            base.Awake();
            GetCoreComponent(ref m_core);
            m_perception = GetComponentInParent<CombatPerception>();

        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            IsTargetInAttackRange = m_perception.CurrentTarget != null &&
            m_perception.CurrentTargetSqrDistance <=
            AttackStartRange * AttackStartRange;
        }

        public void NotifyAttackHit()
        {
            m_core.OnAttackHit();
        }

        public bool TryAttack()
        {
            if (Time.time < m_nextAttackTime)
                return false;

            float attackRate = Mathf.Max(StatsComponent.AttackRate, 0.01f);
            float cooldown = 1f / attackRate;

            m_nextAttackTime = Time.time + cooldown;
            m_core.Prepare(StatsComponent.AttackDamage,this);
            return true;
        }
      

    }
}
