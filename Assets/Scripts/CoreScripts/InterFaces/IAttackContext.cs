using Combat;
using UnityEngine;
using Game;

public interface IAttackContext
{
    Transform OwnerTransform { get; }
    CombatPerception Perception { get; }
    StatsComponent Stats { get; }
    BaseEntity OwnerEntity { get; }
    void ApplyDamage(BaseEntity target, int damage);
}
