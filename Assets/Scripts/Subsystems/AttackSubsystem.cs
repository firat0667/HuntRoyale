using Subsystems.CoreComponents;
using Subsystems.CoreComponents.AttackCores;
using UnityEngine;


namespace Subsystems
{
    public class AttackSubsystem : Subsystem, IAttackContext
    {

        private AttackCore m_core;

        [SerializeField] private MeleeAttackCore m_meleeCore;

        [SerializeField] private RangedAttackCore m_rangedCore;

        private float m_nextAttackTime;

        public bool IsTargetInAttackRange { get; private set; }

        private CombatPerception m_perception;
        public CombatPerception Perception => m_perception;
        public StatsComponent Stats => StatsComponent;
        public Transform CurrentTarget => m_perception.CurrentTarget;

        public float AttackStartRange => StatsComponent.AttackStartRange;
        public float AttackAngle => StatsComponent.AttackAngle;
        public float attackHitRange => StatsComponent.AttackHitRange;   
        public float DetectRange => StatsComponent.DetectionRange;

        protected override void Awake()
        {
            base.Awake();

            m_perception = GetComponentInParent<CombatPerception>();

            switch (StatsComponent.AttackType)
            {
                case AttackType.Melee:
                    m_core = m_meleeCore;
                    if (m_rangedCore != null)
                        m_rangedCore.gameObject.SetActive(false);
                    break;

                case AttackType.Ranged:
                    m_core = m_rangedCore;
                    if (m_meleeCore != null)
                        m_meleeCore.gameObject.SetActive(false);
                    break;
            }

            if (m_core == null)
            {
                Debug.LogError("AttackSubsystem: Active AttackCore is missing!");
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (m_perception.CurrentTarget == null)
            {
                IsTargetInAttackRange = false;
                return;
            }
            float range = GetEffectiveAttackRange();

            IsTargetInAttackRange =
                m_perception.CurrentTargetSqrDistance <= range * range;
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
        private float GetEffectiveAttackRange()
        {
            if (StatsComponent.AttackType == AttackType.Ranged)
                return StatsComponent.ProjectileRange;

            return StatsComponent.AttackStartRange;
        }


    }
}
