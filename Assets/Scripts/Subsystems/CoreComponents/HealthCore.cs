using Firat0667.WesternRoyaleLib.Key;
using UnityEngine;


namespace Subsystems.CoreComponents
{
    public class HealthCore : CoreComponent
    {
        #region Signals
        public BasicSignal OnDeath;
        public BasicSignal<int, int> OnHealthChanged;
        #endregion


        private int m_maxHealth;
        private int m_currentHealth;

        public int CurrentHP => m_currentHealth;
        public int MaxHP => m_maxHealth;

        protected override void Awake()
        {
            base.Awake();
            OnDeath = new BasicSignal();
            OnHealthChanged = new BasicSignal<int, int>();
        }
        private void OnEnable()
        {
            m_currentHealth = m_maxHealth;
        }
        public void Init(int maxHP)
        {
            m_maxHealth = maxHP;
            m_currentHealth = maxHP;
            OnHealthChanged.Emit(m_currentHealth, m_maxHealth);
        }
        public void ApplyDamage(int amount)
        { 
            m_currentHealth -= amount;
            if (m_currentHealth <= 0)
            {
                m_currentHealth = 0;
                OnDeath.Emit();
            }
            OnHealthChanged.Emit(m_currentHealth, m_maxHealth);
        }
        public void ApplyHeal(int amount)
        {
            m_currentHealth = Mathf.Min(m_currentHealth + amount, m_maxHealth);
            OnHealthChanged.Emit(m_currentHealth, m_maxHealth);
        }

    }
}
