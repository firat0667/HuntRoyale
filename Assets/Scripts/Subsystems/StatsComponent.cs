using Combat.Stats.ScriptableObjects;
using Combat.Effects.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private BaseStatsSO m_baseStats;

    public int MaxHP => m_baseStats.maxHP;
    public float DetectionRange => m_baseStats.detectionRange;

    #region Cummon Attack Stats
    public AttackType AttackType => m_baseStats.attackType;
    public int AttackDamage => m_baseStats.attackDamage;
    public float AttackRate => m_baseStats.attackRate;
    public float AttackStartRange => m_baseStats.attackStartRange;
    public float AttackHitRange => m_baseStats.attackHitRange;
    public float AttackAngle => m_baseStats.attackAngle;
    #endregion

    #region Movement Stats
    public float MoveSpeed => m_baseStats.moveSpeed;
    public float MoveAttackSpeedMult => m_baseStats.moveAttackSpeedMult;
    public float RotationSpeed => m_baseStats.rotationSpeed;
    #endregion

    #region Projectile Stats
    public float ProjectileSpeed => m_baseStats.projectileStats.speed;
    public int ProjectilePierce => m_baseStats.projectileStats.maxTargets;
    public float ProjectileRange => m_baseStats.projectileStats.range;
    public int ProjectileID => m_baseStats.projectileStats.projectileID;
    #endregion

    #region Summon Stats
    public int SummonID => m_baseStats.summonStats.summonID;

    public float SummonAttackRange => m_baseStats.summonStats.spawnRange;
    public float SummonExplosionRadius => m_baseStats.summonStats.explosionRadius;
    public float SummonExplosionTriggerDistance=> m_baseStats.summonStats.explosionTriggerDistance;
    public int SummonSpawnCount => m_baseStats.summonStats.spawnCount;

    public float EffectiveAttackRange
    {
        get
        {
            switch (AttackType)
            {
                case AttackType.Ranged:
                    return ProjectileRange;

                case AttackType.Summon:
                    return SummonAttackRange;

                default:
                    return AttackStartRange;
            }
        }
    }

    #endregion

    #region On Hit Effects
    public List<StatusEffectSO> OnHitEffects => m_baseStats.onHitEffects;
    public List<StatusEffectSO> SelfEffects => m_baseStats.selfEffects;
    // add upgrade system later to modify this value
    public float LifeStealPercent => 0.1f;

    #endregion
}


