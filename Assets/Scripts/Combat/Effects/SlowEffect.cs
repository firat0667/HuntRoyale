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

        protected override void OnApply()
        {
            m_effectSubsystem=m_target.GetSubsystem<EffectSubsystem>();
            if (m_effectSubsystem.ActiveEffects
            .Any(e => e is FreezeEffect))
                return;

            m_movement = m_target.GetSubsystem<MovementSubsystem>();
            if (m_movement == null) return;

            float multiplier = 1f - ((SlowEffectSO)m_source).slowPercent;

            m_movement.AddSpeedMultiplier(this, multiplier);
        }

        protected override void OnExpire()
        {
            if (m_movement == null) return;

            m_movement.RemoveSpeedMultiplier(this);
        }

        protected override void OnTick(float tickInterval)
        {
            // Slow doesn't have any ticking behavior 
        }
    }
}
