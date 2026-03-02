using Combat.Effects.ScriptableObjects;
using Subsystems;
using System.Linq;
using UnityEngine;

namespace Combat.Effects
{
    public class SlowEffect : StatusEffect
    {
        private MovementSubsystem m_movement;
        private EffectSubsystem m_effectSubsystem;
        private float m_bonusPercent;
        private object m_key;

        protected override void OnApply()
        {
            var so = (SlowEffectSO)m_source;

            m_effectSubsystem = m_target.GetSubsystem<EffectSubsystem>();
            if (m_effectSubsystem != null && m_effectSubsystem.ActiveEffects.Any(e => e is FreezeEffect))
                return;

            m_movement = m_target.GetSubsystem<MovementSubsystem>();
            if (m_movement == null) return;

            float slowValue = so.slowPercent;
            slowValue *= (1f + m_bonusPercent / 100f);

            slowValue = Mathf.Clamp01(slowValue);

            float multiplier = 1f - slowValue;

            m_key = so;

            m_movement.AddSpeedMultiplier(m_key, multiplier);
        }
        protected override void ApplyPercentBonus(float bonus)
        {
            m_bonusPercent = bonus;
        }
        public override void Refresh(StatusEffect newEffect)
        {
            m_duration = newEffect.Source.duration;
            m_remainingTime = m_duration;
        }
        protected override void OnExpire()
        {
            if (m_movement == null) return;
            if (m_key == null) return;

            m_movement.RemoveSpeedMultiplier(m_key);
        }

        protected override void OnTick(float tickInterval)
        {
           
        }
    }
}
