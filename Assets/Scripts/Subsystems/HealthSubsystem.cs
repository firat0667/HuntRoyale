using Firat0667.CaseLib.Key;
using Subsystems.CoreComponents;
using UnityEngine;

namespace Subsystems
{
    public class HealthSubsystem : Subsystem, IDamageable, IHealth
    {
        private HealthCore m_core;
        public int CurrentHP => m_core.CurrentHP;
        public int MaxHP => m_core.MaxHP;

        #region Signals
        public BasicSignal OnDied;
        public BasicSignal<Transform> OnDamaged  {  get; private set; }

        public BasicSignal OnHealed;
        #endregion

        public bool IsDead => CurrentHP <= 0;

        #region Heal Zone

        private bool m_healable  = true;
        private bool m_inHealZone;
        public bool IsHealable => CurrentHP<MaxHP && m_inHealZone && m_healable ;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            OnDamaged = new BasicSignal<Transform>();
            OnDied = new BasicSignal();
            OnHealed = new BasicSignal();

        }
        private void Start()
        {
            GetCoreComponent(ref m_core);
            m_core.Init(StatsComponent.MaxHP);
            m_core.OnDeath.Connect(HandleDeath);
        }
        public void ResetHealth()
        {
             m_core.Init(StatsComponent.MaxHP);
        }
        public void TakeDamage(int amount, Transform source)
        {
            Debug.Log($"{transform.parent.parent.name} took {amount} damage. current health{CurrentHP} ");
            m_core.ApplyDamage(amount);
            OnDamaged.Emit(source);
        }
        public void Heal(int amount)
        {
            if (!IsHealable)
                return;
             
            m_core.ApplyHeal(amount);
            OnHealed.Emit();
            Debug.Log($"{transform.parent.parent.name} healed {amount} health. current health{CurrentHP} ");
        }
        public void SetHealable(bool value)
        {
            m_healable = value;
        }
        public void SetInHealZone(bool value)
        {
            m_inHealZone = value;
        }
        private void HandleDeath()
        {
            OnDied.Emit();
        }
    }
}
