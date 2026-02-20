using Combat.Stats.ScriptableObjects;
using Combat.Effects.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Combat.Upgrades.ScriptableObjects;

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private BaseStatsSO m_baseStats;

    public BaseStatsSO BaseStats => m_baseStats;

    public int MaxHP => m_baseStats.maxHP+m_bonusMaxHP;
    public float DetectionRange => m_baseStats.detectionRange;

    #region Cummon Attack Stats
    public AttackType AttackType => m_baseStats.attackType;
    public int AttackDamage => m_baseStats.attackDamage+m_bonusDamage;
    public float AttackRate => m_baseStats.attackRate+m_bonusAttackRate;
    public float AttackStartRange => m_baseStats.attackStartRange+m_bonusRange;
    public float AttackHitRange => m_baseStats.attackHitRange;
    public float AttackAngle => m_baseStats.attackAngle + m_bonusAttackAngle;
    #endregion

    #region Movement Stats
    public float MoveSpeed => m_baseStats.moveSpeed;
    public float MoveAttackSpeedMult => m_baseStats.moveAttackSpeedMult;
    public float RotationSpeed => m_baseStats.rotationSpeed;
    #endregion

    #region Projectile Stats
    public float ProjectileSpeed => m_baseStats.projectileStats.speed;
    public int ProjectilePierce => m_baseStats.projectileStats.maxTargets+m_bonusProjectilePierce;
    public float ProjectileRange => m_baseStats.projectileStats.range;
    public int ProjectileID => m_baseStats.projectileStats.projectileID;
    #endregion

    #region Summon Stats
    public int SummonID => m_baseStats.summonStats.summonID;
    public float SummonLifeTime => m_baseStats.summonStats.lifeTime+m_bonusSummonLifetime;
    public float SummonAttackRange => m_baseStats.summonStats.spawnRange;
    public float SummonExplosionRadius => m_baseStats.summonStats.explosionRadius+ m_bonusSummonExplosionRadius;
    public float SummonExplosionTriggerDistance=> m_baseStats.summonStats.explosionTriggerDistance;
    public int SummonSpawnCount => m_baseStats.summonStats.spawnCount+m_bonusSummonCount;

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

    #region Upgrade Stats
    private int m_bonusMaxHP;
    private int m_bonusDamage;
    private float m_bonusAttackRate;
    private float m_bonusRange;
    private int m_bonusAttackAngle;

    private int m_bonusProjectilePierce;

    private int m_bonusSummonCount;
    private float m_bonusSummonLifetime;
    private float m_bonusSummonExplosionRadius;

    private float m_bonusExpPercent;
    private int m_bonusFlatExp;

    private Dictionary<StatusEffectSO, float> m_bonusDuration = new();
    private Dictionary<StatusEffectSO, float> m_bonusPercent = new();
    private Dictionary<StatusEffectSO, float> m_bonusTickInterval = new();

    #endregion

    #region On Hit Effects
    public List<StatusEffectSO> OnHitEffects => m_baseStats.onHitEffects;
    public List<StatusEffectSO> SelfEffects=> m_baseStats.selfEffects;
    public float LifeStealPercent => 0.1f;

    #endregion

    public void ApplyUpgradeStat(UpgradeSO upgrade)
    {
        switch (upgrade.statType)
        {
            case StatType.Exp:
                if (upgrade.isPercentage)
                    m_bonusExpPercent += upgrade.value;
                else
                    m_bonusFlatExp += (int)upgrade.value;
                break;
            case StatType.Health:
                m_bonusMaxHP += (int)upgrade.value;
                break;

            case StatType.Damage:
                m_bonusDamage += (int)upgrade.value;
                break;

            case StatType.AttackSpeed:
                m_bonusAttackRate += upgrade.value;
                break;

            case StatType.Range:
                m_bonusRange += upgrade.value;
                break;

            case StatType.ProjectileMaxTargets:
                m_bonusProjectilePierce += (int)upgrade.value;
                break;

            case StatType.MinionCount:
                m_bonusSummonCount += (int)upgrade.value;
                break;

            case StatType.MinionLifetime:
                m_bonusSummonLifetime += upgrade.value;
                break;

            case StatType.ExplosionRadius:
                m_bonusSummonExplosionRadius += upgrade.value;
                break;
            case StatType.Duration:
                if (upgrade.targetEffect != null)
                {
                    if (!m_bonusDuration.ContainsKey(upgrade.targetEffect))
                        m_bonusDuration[upgrade.targetEffect] = 0f;

                    m_bonusDuration[upgrade.targetEffect] += upgrade.value;
                }
                break;

            case StatType.TickInterval:
                if (upgrade.targetEffect != null)
                {
                    if (!m_bonusTickInterval.ContainsKey(upgrade.targetEffect))
                        m_bonusTickInterval[upgrade.targetEffect] = 0f;

                    m_bonusTickInterval[upgrade.targetEffect] += upgrade.value;
                }
                break;

            case StatType.Percent:
                if (upgrade.targetEffect != null)
                {
                    if (!m_bonusPercent.ContainsKey(upgrade.targetEffect))
                        m_bonusPercent[upgrade.targetEffect] = 0f;

                    m_bonusPercent[upgrade.targetEffect] += upgrade.value;
                }
                break;
        }
    }
    public void GetEffectBonuses(StatusEffectSO so,
        out float bonusDuration,
        out float bonusTick,
        out float bonusPercent)
    {
        bonusDuration = 0f;
        bonusTick = 0f;
        bonusPercent = 0f;

        if (so == null) return;

        if (m_bonusDuration.TryGetValue(so, out var d))
            bonusDuration = d;

        if (m_bonusTickInterval.TryGetValue(so, out var t))
            bonusTick = t;

        if (m_bonusPercent.TryGetValue(so, out var p))
            bonusPercent = p;
    }
    public int ModifyExp(int baseExp)
    {
        int flat = m_bonusFlatExp;
        float percent = m_bonusExpPercent;

        float total = (baseExp + flat) * (1f + percent);
        return Mathf.RoundToInt(total);
    }

}


