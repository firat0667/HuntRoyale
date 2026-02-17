using Combat.Effects.ScriptableObjects;
using UnityEngine;

namespace Combat.Effects.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Effects/Slow")]
    public class SlowEffectSO : StatusEffectSO
    {
        [Range(0f, 1f)]
        public float slowPercent = 0.3f; 

        public override StatusEffect CreateEffect(float damageDealt)
        {
            return new SlowEffect();
        }
    }
}