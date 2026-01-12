using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Base Entity Stats")]
public class BaseStatsSO : ScriptableObject
{
    [Header("Health")]
    public float maxHP;

    [Header("Combat")]
    public float attackDamage;
    public float attackRate;
    public float attackRange;

    [Header("Movement")]
    public float moveSpeed;

    [Header("Rotation")]
    public float rotationSpeed;
}
