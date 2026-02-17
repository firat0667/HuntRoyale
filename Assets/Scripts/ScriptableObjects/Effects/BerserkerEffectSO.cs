using UnityEngine;

namespace Combat.Effects.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Effects/Berserker (Lifesteal Buff)")]
    public class BerserkerEffectSO : StatusEffectSO
    {
        [Range(0f, 1f)]
        public float baseLifeStealPercent = 0.2f;

        public override StatusEffect CreateEffect(float damageDealt)
        {
            return new BerserkerEffect();
        }
    }
}