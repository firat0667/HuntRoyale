using UnityEngine;

namespace Combat.Effects.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Effects/Freeze")]
    public class FreezeEffectSO : StatusEffectSO
    {
        public override StatusEffect CreateEffect(float damageDealt)
        {
            return new FreezeEffect();
        }
    }
}