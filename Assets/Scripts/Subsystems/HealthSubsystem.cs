using Firat0667.CaseLib.Key;
using Subsystems.CoreComponents;
using UnityEngine;

namespace Subsystems
{
    public class HealthSubsystem : Subsystem, IDamageable, IHealth
    {
        public BasicSignal OnDied;

        private HealthCore m_core;
        public int CurrentHP => m_core.CurrentHP;
        public int MaxHP => m_core.MaxHP;

        protected override void Awake()
        {
            base.Awake();

        }
        private void Start()
        {
            OnDied = new BasicSignal();
            GetCoreComponent(ref m_core);
            m_core.Init(StatsComponent.MaxHP);
            m_core.OnDeath.Connect(HandleDeath);
        }
        public void TakeDamage(int amount)
        {
            m_core.ApplyDamage(amount);
        }
        public void Heal(int amount)
        {
            m_core.ApplyHeal(amount);
        }
        private void HandleDeath()
        {
            OnDied.Emit();
        }
    }
}
