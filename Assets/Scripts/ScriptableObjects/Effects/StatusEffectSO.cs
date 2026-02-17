using Combat.Effects;
using UnityEngine;

namespace Combat.Effects.ScriptableObjects
{
    public abstract class StatusEffectSO : ScriptableObject
    {
        public float duration = 5f;

        [Header("Tick (Burn / Poison only)")]
        public float tickInterval = 1f;

        public Sprite icon;

        public abstract StatusEffect CreateEffect(float damageDealt);
    }
}