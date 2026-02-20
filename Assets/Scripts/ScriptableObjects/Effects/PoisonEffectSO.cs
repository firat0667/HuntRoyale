using UnityEngine;

namespace Combat.Effects.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Effects/Poison")]
    public class PoisonEffectSO : StatusEffectSO
    {
        [Range(0f, 1f)]
        public float damagePercent = 0.05f;

        public override StatusEffect CreateEffect(float damageDealt)
        {
            return new PoisonEffect(damageDealt);
        }
    }
}