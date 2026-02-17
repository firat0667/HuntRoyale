using Combat.Effects.ScriptableObjects;
using Subsystems;
using UnityEngine;

namespace Combat.Effects
{
    public class PoisonEffect : StatusEffect
    {
        private float m_damagePerSecond;

        public PoisonEffect(float damageDealt, float percent, float duration)
        {
            float totalDamage = damageDealt * percent;

            if (totalDamage < 1f)
                totalDamage = 1f;

            m_damagePerSecond = totalDamage / duration;

            m_duration = duration;
            m_remainingTime = duration;
        }

        protected override void OnApply() { }

        protected override void OnTick(float tickInterval)
        {
            if (m_health == null || m_target.IsDead)
                return;

            int damage = Mathf.RoundToInt(m_damagePerSecond * tickInterval);

            if (damage < 1)
                damage = 1;

            m_health.TakeDamage(damage, null);
        }

        protected override void OnExpire() { }
    }
}