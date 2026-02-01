using Combat;
using UnityEngine;

public interface IAttackContext
{
    Transform OwnerTransform { get; }
    CombatPerception Perception { get; }
    StatsComponent Stats { get; }
}
