using Combat.Effects.ScriptableObjects;
using Subsystems;
using UnityEngine;

namespace Combat.Effects
{
    public abstract class StatusEffect
    {
        protected BaseEntity m_target;
        protected HealthSubsystem m_health;
        protected StatusEffectSO m_source;

        protected float m_duration;
        protected float m_remainingTime;

        protected float m_tickInterval;
        private float m_tickTimer;

        private bool m_hasExpired;

        public float RemainingTime => m_remainingTime;
        public bool IsFinished => m_remainingTime <= 0;
        public StatusEffectSO Source => m_source;

        public void Init(BaseEntity target, StatusEffectSO source)
        {
            m_target = target;
            m_health = target.GetSubsystem<HealthSubsystem>();
            m_source = source;

            m_duration = source.duration;
            m_remainingTime = m_duration;
            m_tickInterval = source.tickInterval;

            OnApply();
        }

        public void Tick(float deltaTime)
        {
            if (IsFinished)
                return;

            m_remainingTime -= deltaTime;
            m_tickTimer += deltaTime;

            while (m_tickTimer >= m_tickInterval)
            {
                m_tickTimer -= m_tickInterval;
                OnTick(m_tickInterval);
            }
        }
        public virtual void Refresh(StatusEffect newEffect)
        {
            m_remainingTime = Mathf.Max(m_remainingTime, newEffect.m_duration);
        }
        public void ForceExpire()
        {
            if (m_hasExpired)
                return;

            m_hasExpired = true;
            OnExpire();
        }

        protected abstract void OnApply();
        protected abstract void OnTick(float tickInterval);
        protected abstract void OnExpire();
    }
}