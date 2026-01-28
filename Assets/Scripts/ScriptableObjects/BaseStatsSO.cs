using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Base Entity Stats")]
public class BaseStatsSO : ScriptableObject
{
    [Header("Health")]
    public int maxHP;

    [Header("Combat")]
    public int   attackDamage;
    public float attackRate;
    public float detectionRange;
    public float attackRange;

    [Header("Movement")]
    public float moveSpeed;

    [Header("Rotation")]
    public float rotationSpeed;
}
