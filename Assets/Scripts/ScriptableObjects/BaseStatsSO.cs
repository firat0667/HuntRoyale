using Combat;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Base Entity Stats")]
public class BaseStatsSO : ScriptableObject
{
    public AttackType attackType = AttackType.Melee;

    public int maxHP;

    public float attackStartRange;
    public float attackHitRange;

    public ProjectileSO projectileStats;

    public int   attackDamage;
    public float attackRate;
    public float attackAngle;
    public float detectionRange;
  
    
    public float moveSpeed;
    public float moveAttackSpeedMult;

    public float rotationSpeed;
}
public enum AttackType
{
    Melee,
    Ranged
}
