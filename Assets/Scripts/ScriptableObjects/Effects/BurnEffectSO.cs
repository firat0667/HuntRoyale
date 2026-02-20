using Combat.Effects;
using UnityEngine;

namespace Combat.Effects.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Effects/Burn")]
    public class BurnEffectSO : StatusEffectSO
    {
        [Range(0f, 1f)]
        public float percent = 0.05f;

        public override StatusEffect CreateEffect(float damageDealt)
        {
            return new BurnEffect(damageDealt);
        }
    }
}