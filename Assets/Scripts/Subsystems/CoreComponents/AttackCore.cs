using UnityEngine;

public class AttackCore : CoreComponent
{
    public virtual void Attack(float damage)
    {
        // Player: use bomb
        // Enemy: melee attack
        // Boss: special attack
        Debug.Log($"{nameof(AttackCore)}.Attack called with damage: {damage}");
    }
}
