using Combat.Effects.ScriptableObjects;
using Combat.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Stats.ScriptableObjects
{

    [CreateAssetMenu(menuName = "Stats/Base Entity Stats")]
    public class BaseStatsSO : ScriptableObject
    {
        public AttackType attackType = AttackType.Melee;

        public int maxHP;

        public float attackStartRange;
        public float attackHitRange;

        public ProjectileSO projectileStats;
        public SummonSO summonStats;


        public int attackDamage;
        public float attackRate;
        public float attackAngle;
        public float detectionRange;


        public float moveSpeed;
        public float moveAttackSpeedMult;

        public float rotationSpeed;

        public List<StatusEffectSO> onHitEffects;
        public List<StatusEffectSO> selfEffects;
    }
    public enum AttackType
    {
        Melee,
        Ranged,
        Summon
    }
}
