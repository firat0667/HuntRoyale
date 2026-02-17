using Subsystems;

namespace Combat.Effects
{
    public class FreezeEffect : StatusEffect
    {
        private MovementSubsystem m_movement;

        protected override void OnApply()
        {
            m_movement = m_target.GetSubsystem<MovementSubsystem>();
            if (m_movement == null) return;

            m_movement.AddSpeedMultiplier(this, 0f);
        }

        protected override void OnTick(float tickInterval)
        {
        }

        protected override void OnExpire()
        {
            if (m_movement == null) return;

            m_movement.RemoveSpeedMultiplier(this);
        }
    }
}