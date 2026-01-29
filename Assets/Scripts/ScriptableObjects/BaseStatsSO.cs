using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Base Entity Stats")]
public class BaseStatsSO : ScriptableObject
{
    public AttackType attackType = AttackType.Melee;

    public int maxHP;

    public float attackStartRange;
    public float attackHitRange;

    public float projectileRange;
    public float projectileSpeed;
    public int projectilePierce;
    public GameObject projectilePrefab;

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
