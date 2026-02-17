using Combat.Effects.ScriptableObjects;
using Subsystems;

namespace Combat.Effects
{
    public class BerserkerEffect : StatusEffect, ILifeStealProvider
    {
        private float m_lifeStealPercent;
        protected override void OnApply()
        {
            var so = (BerserkerEffectSO)m_source;

            var stats = m_target.GetSubsystem<AttackSubsystem>().Stats;

            float statMultiplier = stats != null ? stats.LifeStealPercent : 1f;

            m_lifeStealPercent = so.baseLifeStealPercent * statMultiplier;
        }
        public float GetLifeStealPercent()
        {
            return m_lifeStealPercent;
        }
        protected override void OnTick(float tickInterval) { }

        protected override void OnExpire() { }
    }
}