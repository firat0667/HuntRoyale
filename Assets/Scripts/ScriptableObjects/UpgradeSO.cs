using Combat.Effects.ScriptableObjects;
using UnityEngine;

namespace Combat.Upgrades.ScriptableObjects
{
    public enum UpgradeCategory
    {
        Global,
        Ranged,
        Melee,
        Summon,
        Effect
    }

    public enum StatType
    {
        Damage,
        AttackSpeed,
        Range,
        Exp,
        DamageAngle,

        ProjectileMaxTargets,

        MinionCount,
        MinionLifetime,
        ExplosionRadius,

        Duration,
        TickInterval,
        Percent,
        SlowPercent
    }

    [CreateAssetMenu(menuName = "Upgrade/Upgrade")]
    public class UpgradeSO : ScriptableObject
    {
        public Sprite icon;

        public UpgradeCategory category;

        public StatType statType;
        public float value;
        public bool isPercentage;

        public StatusEffectSO targetEffect;

        public int maxStack = 5;

        [Range(1, 100)]
        public int weight = 10;
    }
}