using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private BaseStatsSO m_baseStats;

    public float MaxHP => m_baseStats.maxHP;
    public float AttackDamage => m_baseStats.attackDamage;
    public float AttackRate => m_baseStats.attackRate;
    public float AttackRange => m_baseStats.attackRange;
    public float MoveSpeed => m_baseStats.moveSpeed;
}
