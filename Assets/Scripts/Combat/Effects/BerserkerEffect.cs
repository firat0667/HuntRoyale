using Combat.Effects.ScriptableObjects;
using Subsystems;

namespace Combat.Effects
{
    public class BerserkerEffect : StatusEffect, ILifeStealProvider
    {
        private float m_lifeStealPercent;
        private float m_bonusPercent;
        protected override void OnApply()
        {
            var so = (BerserkerEffectSO)m_source;

            var stats = m_target.GetSubsystem<AttackSubsystem>().Stats;

            float statMultiplier = stats != null ? stats.LifeStealPercent : 1f;

            float baseValue = so.baseLifeStealPercent * statMultiplier;

            m_lifeStealPercent = baseValue * (1f + m_bonusPercent / 100f);
        }
        protected override void ApplyPercentBonus(float bonus)
        {
            m_bonusPercent = bonus;
        }
        public float GetLifeStealPercent()
        {
            return m_lifeStealPercent;
        }
        protected override void OnTick(float tickInterval) { }

        protected override void OnExpire() { }
    }
}