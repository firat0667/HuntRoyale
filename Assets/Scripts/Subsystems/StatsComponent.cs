using Combat;
using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private BaseStatsSO m_baseStats;

    public int MaxHP => m_baseStats.maxHP;
    public float DetectionRange => m_baseStats.detectionRange;


    public AttackType AttackType => m_baseStats.attackType;
    public int AttackDamage => m_baseStats.attackDamage;
    public float AttackRate => m_baseStats.attackRate;
    public float AttackStartRange => m_baseStats.attackStartRange;
    public float AttackHitRange => m_baseStats.attackHitRange;
    public float AttackAngle => m_baseStats.attackAngle;


    public float MoveSpeed => m_baseStats.moveSpeed;
    public float MoveAttackSpeedMult => m_baseStats.moveAttackSpeedMult;
    public float RotationSpeed => m_baseStats.rotationSpeed;


    public float ProjectileSpeed => m_baseStats.projectileStats.Speed;
    public int ProjectilePierce => m_baseStats.projectileStats.MaxTargets;
    public float ProjectileRange => m_baseStats.projectileStats.Range;
    public int ProjectileID => m_baseStats.projectileStats.ProjectileID;
}


