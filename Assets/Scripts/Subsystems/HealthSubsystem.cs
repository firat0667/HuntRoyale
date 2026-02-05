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
        public BasicSignal<Transform> OnDamaged  {  get; private set; }

        protected override void Awake()
        {
            base.Awake();
            OnDamaged = new BasicSignal<Transform>();
            OnDied = new BasicSignal();

        }
        private void Start()
        {
            GetCoreComponent(ref m_core);
            m_core.Init(StatsComponent.MaxHP);
            m_core.OnDeath.Connect(HandleDeath);
        }
        public void TakeDamage(int amount, Transform source)
        {
            Debug.Log($"{transform.parent.parent.name} took {amount} damage. current health{CurrentHP} ");
            //change debug later
            m_core.ApplyDamage(amount);
            OnDamaged.Emit(source);
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
