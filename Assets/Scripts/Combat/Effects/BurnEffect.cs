using Combat.Effects.ScriptableObjects;
using Subsystems;
using UnityEngine;

namespace Combat.Effects
{
    public class BurnEffect : StatusEffect
    {
        private float m_damagePerSecond;
        private float m_baseDamageDealt;
        private float m_bonusPercent;

        public BurnEffect(float damageDealt)
        {
            m_baseDamageDealt = damageDealt;
        }

        protected override void ApplyPercentBonus(float bonus)
        {
            m_bonusPercent = bonus;
        }

        protected override void OnApply()
        {
            var so = (BurnEffectSO)m_source;

            float totalDamage = m_baseDamageDealt * so.percent;

            totalDamage *= (1f + m_bonusPercent / 100f);

            if (totalDamage < 1f)
                totalDamage = 1f;

            m_damagePerSecond = totalDamage / m_duration;
        }

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