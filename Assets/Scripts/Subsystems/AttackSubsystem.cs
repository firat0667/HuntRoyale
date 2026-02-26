using Combat;
using Combat.Effects.ScriptableObjects;
using Combat.Stats.ScriptableObjects;
using Game;
using Subsystems.CoreComponents;
using Subsystems.CoreComponents.AttackCores;
using System.Collections.Generic;
using UnityEngine;


namespace Subsystems
{
    public class AttackSubsystem : Subsystem, IAttackContext
    {

        #region Core Components
        private AttackCore m_core;

        private MeleeAttackCore m_meleeCore;

        private RangedAttackCore m_rangedCore;

        private SummonAttackCore m_summonCore;
        #endregion

        private float m_nextAttackTime;
        public bool IsTargetInAttackRange { get; private set; }

        private CombatPerception m_perception;
        public CombatPerception Perception => m_perception;
        #region Properties
        public StatsComponent Stats => StatsComponent;
        public Transform OwnerTransform => OwnerEntity != null ? 
         OwnerEntity.transform : transform.root;
        public Transform CurrentTarget => m_perception.CurrentTarget;
        public BaseEntity OwnerEntity => GetComponentInParent<BaseEntity>();
        public float AttackStartRange => StatsComponent.AttackStartRange;
        public float AttackAngle => StatsComponent.AttackAngle;
        public float attackHitRange => StatsComponent.AttackHitRange;   
        public float DetectRange => StatsComponent.DetectionRange;
        #endregion

        public float EffectiveAttackRange { get; private set; }
        protected override void Awake()
        {
            base.Awake();

            m_perception = GetComponentInParent<CombatPerception>();

            m_meleeCore = GetComponentInChildren<MeleeAttackCore>(true);
            m_rangedCore = GetComponentInChildren<RangedAttackCore>(true);
            m_summonCore = GetComponentInChildren<SummonAttackCore>(true);

            if (m_meleeCore != null) m_meleeCore.gameObject.SetActive(false);
            if (m_rangedCore != null) m_rangedCore.gameObject.SetActive(false);
            if (m_summonCore != null) m_summonCore.gameObject.SetActive(false);

            switch (StatsComponent.AttackType)
            {
                case AttackType.Melee:
                    m_core = m_meleeCore;
                    break;

                case AttackType.Ranged:
                    m_core = m_rangedCore;
                    break;

                case AttackType.Summon:
                    m_core = m_summonCore;
                    break;
            }
            if (m_core == null)
            {
                Debug.LogError("AttackSubsystem: Active AttackCore is missing!");
                return;
            }

            m_core.gameObject.SetActive(true);
        }
        public bool CanAttack()
        {
            if (CurrentTarget == null)
                return false;

            var entry = CurrentTarget.GetComponentInParent<BaseEntity>();
            if (entry == null|| entry.IsDead)              
            {
                m_perception.ClearTarget();
                return false;
            }

            if (!IsTargetInAttackRange)
                return false;

            if (!m_perception.HasClearLineOfSight(CurrentTarget))
                return false;

            return true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (m_perception.CurrentTarget == null)
            {
                IsTargetInAttackRange = false;
                EffectiveAttackRange = 0f;
                return;
            }
            EffectiveAttackRange = StatsComponent.EffectiveAttackRange;

            IsTargetInAttackRange =
                m_perception.CurrentTargetSqrDistance <= EffectiveAttackRange * EffectiveAttackRange;
        }

        public void NotifyAttackHit()
        {
            m_core.OnAttackHit();
        }
        public void ApplyDamage(BaseEntity target, int damage)
        {
            target.Health.TakeDamage(damage, OwnerTransform);
            ApplyEffects(OwnerEntity, Stats.SelfEffects, damage);
            OwnerEntity?.OnDealDamage(damage);
            ApplyEffects(target, Stats.OnHitEffects, damage);
        }
        private void ApplyEffects(BaseEntity target,
                          List<StatusEffectSO> effects,
                          int damage)
        {
            if (effects == null) return;

            foreach (var so in effects)
            {
                var effect = so.CreateEffect(damage);

                float bonusDuration = 0f;
                float bonusTick = 0f;
                float bonusPercent = 0f;

                StatsComponent.GetEffectBonuses(so,
                    out bonusDuration,
                    out bonusTick,
                    out bonusPercent);

                effect.Init(target, so,
                    bonusDuration,
                    bonusTick,
                    bonusPercent);

                target.ApplyEffect(effect, so);
            }
        }
        public bool TryAttack()
        {
            if (!CanAttack())
                return false;

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
