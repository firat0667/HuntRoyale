using Subsystems;
using UnityEngine;

namespace Combat.Effects
{
    public class FreezeEffect : StatusEffect
    {
        private MovementSubsystem m_movement;
        private object m_key;

        protected override void OnApply()
        {
            m_movement = m_target.GetSubsystem<MovementSubsystem>();
            if (m_movement == null) return;

            m_key = m_source;

            m_movement.AddSpeedMultiplier(m_key, 0f);
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