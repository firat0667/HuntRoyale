using CoreScripts.ObjectPool;
using Firat0667.WesternRoyaleLib.Key;
using Subsystems.CoreComponents;
using UnityEngine;

namespace Subsystems
{
    public class HealthSubsystem : Subsystem, IDamageable, IHealth
    {
        #region Properties

        public int ExpReward = 5; // change this value as needed for balancing  also add xp starts to the enemy prefab

        private HealthCore m_core;
        private bool m_deathHandled;
        public int CurrentHP => m_core.CurrentHP;
        public int MaxHP => m_core.MaxHP;

        private Transform m_lastDamageSource;
        public Transform LastDamageSource => m_lastDamageSource;
        #endregion
        #region Signals
        public BasicSignal OnDied;
        public BasicSignal<Transform> OnDamaged  {  get; private set; }

        public BasicSignal OnHealed;
        public BasicSignal<int, int> OnHealthChanged => m_core.OnHealthChanged;


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
            GetCoreComponent(ref m_core);
          
        }
        private void Start()
        {
            m_core.Init(StatsComponent.MaxHP);
            m_core.OnDeath.Connect(HandleDeath);
            StatsComponent.OnMaxHealthChanged.Connect(ResetHealth);
        }
        private void OnDisable()
        {
            StatsComponent.OnMaxHealthChanged.Disconnect(ResetHealth);
        }
        public void ResetHealth()
        {
             m_core.Init(StatsComponent.MaxHP);
        }
        public void TakeDamage(int amount, Transform source)
        {
            Debug.Log($"{transform.parent.parent.name} took {amount} damage. current health{CurrentHP} ");
            m_lastDamageSource = source;
            m_core.ApplyDamage(amount);
            OnDamaged.Emit(source);
            DamagePopupPool.Instance.Spawn(amount, transform.position,false);
        }
        public void Heal(int amount)
        {
            if (!m_healable)
                return;
             
            m_core.ApplyHeal(amount);
            OnHealed.Emit();
            Debug.Log($"{transform.parent.parent.name} healed {amount} health. current health{CurrentHP} ");
            DamagePopupPool.Instance.Spawn(amount, transform.position,true);
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
