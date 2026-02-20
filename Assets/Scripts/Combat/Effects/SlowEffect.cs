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

        protected override void OnApply()
        {
            float slowValue = ((SlowEffectSO)m_source).slowPercent;

            slowValue *= (1f + m_bonusPercent / 100f);

            float multiplier = 1f - slowValue;

            m_effectSubsystem =m_target.GetSubsystem<EffectSubsystem>();
            if (m_effectSubsystem.ActiveEffects
            .Any(e => e is FreezeEffect))
                return;

            m_movement = m_target.GetSubsystem<MovementSubsystem>();
            if (m_movement == null) return;

            m_movement.AddSpeedMultiplier(this, multiplier);
        }
        protected override void ApplyPercentBonus(float bonus)
        {
            m_bonusPercent = bonus;
        }
        protected override void OnExpire()
        {
            if (m_movement == null) return;

            m_movement.RemoveSpeedMultiplier(this);
        }

        protected override void OnTick(float tickInterval)
        {
           
        }
    }
}
