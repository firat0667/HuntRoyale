using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private BaseStatsSO m_baseStats;

    public int MaxHP => m_baseStats.maxHP;
    public int AttackDamage => m_baseStats.attackDamage;
    public float AttackRate => m_baseStats.attackRate;
    public float AttackStartRange => m_baseStats.attackStartRange;
    public float AttackHitRange => m_baseStats.attackHitRange;
    public float AttackAngle => m_baseStats.attackAngle;
    public float MoveSpeed => m_baseStats.moveSpeed;
    public float MoveAttackSpeedMult => m_baseStats.moveAttackSpeedMult;
    public float RotationSpeed => m_baseStats.rotationSpeed;
    public float DetectionRange => m_baseStats.detectionRange;
}
