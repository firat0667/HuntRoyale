using Combat.Stats.ScriptableObjects;
using Combat.Effects.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Combat.Upgrades.ScriptableObjects;
using Firat0667.WesternRoyaleLib.Key;

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private BaseStatsSO m_baseStats;

    private BasicSignal m_onMaxHealthChanged;
    public BasicSignal OnMaxHealthChanged => m_onMaxHealthChanged;
    public BaseStatsSO BaseStats => m_baseStats;
    public int MaxHP =>
    Mathf.RoundToInt(GetStat(StatType.Health, m_baseStats.maxHP));
    public float DetectionRange => m_baseStats.detectionRange;

    #region Cummon Attack Stats
    public AttackType AttackType => m_baseStats.attackType;
    public int AttackDamage =>
    Mathf.RoundToInt(GetStat(StatType.Damage, m_baseStats.attackDamage));
    public float AttackRate =>
    GetStat(StatType.AttackSpeed, m_baseStats.attackRate);
    public float AttackStartRange =>
    GetStat(StatType.Range, m_baseStats.attackStartRange);
    public float AttackHitRange => GetStat(StatType.Range, m_baseStats.attackHitRange);
    public float AttackAngle => GetStat(StatType.DamageAngle, m_baseStats.attackAngle);
    #endregion

    #region Movement Stats
    public float MoveSpeed => m_baseStats.moveSpeed;
    public float MoveAttackSpeedMult => m_baseStats.moveAttackSpeedMult;
    public float RotationSpeed => m_baseStats.rotationSpeed;
    #endregion

    #region Projectile Stats
    public float ProjectileSpeed => m_baseStats.projectileStats.speed;
    public int ProjectilePierce =>
    Mathf.RoundToInt(GetStat(StatType.ProjectileMaxTargets,
        m_baseStats.projectileStats.maxTargets));
    public float ProjectileRange =>
     GetStat(StatType.Range, m_baseStats.projectileStats.range);
    public int ProjectileID => m_baseStats.projectileStats.projectileID;
    #endregion

    #region Summon Stats
    public int SummonID => m_baseStats.summonStats.summonID;
    public float SummonLifeTime =>
    GetStat(StatType.MinionLifetime,
        m_baseStats.summonStats.lifeTime);
    public float SummonAttackRange => m_baseStats.summonStats.spawnRange;
    public float SummonExplosionRadius => GetStat(StatType.ExplosionRadius,m_baseStats.summonStats.explosionRadius);
    public float SummonExplosionTriggerDistance=> GetStat(StatType.ExplosionRadius, m_baseStats.summonStats.explosionTriggerDistance);
    public int SummonSpawnCount =>
    Mathf.RoundToInt(GetStat(StatType.MinionCount,
        m_baseStats.summonStats.spawnCount));

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

 

    private Dictionary<StatType, float> m_flatStats = new();
    private Dictionary<StatType, float> m_percentStats = new();

    private Dictionary<StatusEffectSO, float> m_bonusDuration = new();
    private Dictionary<StatusEffectSO, float> m_bonusPercent = new();
    private Dictionary<StatusEffectSO, float> m_bonusTickInterval = new();


    #region On Hit Effects
    public List<StatusEffectSO> OnHitEffects => m_baseStats.onHitEffects;
    public List<StatusEffectSO> SelfEffects=> m_baseStats.selfEffects;
    public float LifeStealPercent => 0.1f;

    #endregion

    private void Awake()
    {
        m_onMaxHealthChanged 
            
            = new BasicSignal();
    }

    public void ApplyUpgradeStat(UpgradeSO upgrade)
    {
        if (upgrade.statType == StatType.Duration ||
            upgrade.statType == StatType.TickInterval ||
            upgrade.statType == StatType.DamagePercent)
        {
            ApplyEffectUpgrade(upgrade);
            return;
        }

        var dict = upgrade.isPercentage ? m_percentStats : m_flatStats;

        if (!dict.ContainsKey(upgrade.statType))
            dict[upgrade.statType] = 0f;

        dict[upgrade.statType] += upgrade.value;

        if (upgrade.statType == StatType.Health)
            OnMaxHealthChanged.Emit();
    }
    private void ApplyEffectUpgrade(UpgradeSO upgrade)
    {
        if (upgrade.targetEffect == null)
            return;

        switch (upgrade.statType)
        {
            case StatType.Duration:
                if (!m_bonusDuration.ContainsKey(upgrade.targetEffect))
                    m_bonusDuration[upgrade.targetEffect] = 0f;

                m_bonusDuration[upgrade.targetEffect] += upgrade.value;
                break;

            case StatType.TickInterval:
                if (!m_bonusTickInterval.ContainsKey(upgrade.targetEffect))
                    m_bonusTickInterval[upgrade.targetEffect] = 0f;

                m_bonusTickInterval[upgrade.targetEffect] += upgrade.value;
                break;

            case StatType.DamagePercent:
                if (!m_bonusPercent.ContainsKey(upgrade.targetEffect))
                    m_bonusPercent[upgrade.targetEffect] = 0f;

                m_bonusPercent[upgrade.targetEffect] += upgrade.value;
                break;
        }
    }
    private float GetStat(StatType type, float baseValue)
    {
        m_flatStats.TryGetValue(type, out var flat);
        m_percentStats.TryGetValue(type, out var percent);

        return (baseValue + flat) * (1f + percent/100);
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
        float result = GetStat(StatType.Exp, baseExp);
        return Mathf.RoundToInt(result);
    }

}


