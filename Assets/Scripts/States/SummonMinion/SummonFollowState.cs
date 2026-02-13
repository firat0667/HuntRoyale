using Combat;
using Pathfinding;
using Subsystems.Ai;
using UnityEngine;

public class SummonFollowState : IState
{
    private readonly SummonMinion s;
    private readonly AINavigationSubsystem m_navigation;
    private readonly AIPath m_aiPath;

    public SummonFollowState(SummonMinion summon)
    {
        s = summon;
        m_navigation = summon.Navigation;
        m_aiPath = summon.AIPath;
    }

    public void Enter()
    {
        m_navigation.SetStopDistance(s.ExplosionTriggerDistance);
    }
    public void LogicUpdate()
    {
        if (s.Owner == null)
            return;

        Transform target = s.CurrentTarget;

        if (target != null &&  target.gameObject.activeInHierarchy)
        {
            m_navigation.SetTarget(target);

            float sqrDist =
                (s.transform.position - target.position).sqrMagnitude;

            if (sqrDist <= s.ExplosionTriggerDistance * s.ExplosionTriggerDistance)
            {
                s.ExplodeNow();
            }
            return;
        }
        float ownerSqrDist =
        (s.transform.position - s.Owner.position).sqrMagnitude;

        float followDistance = 2f;

        if (ownerSqrDist > followDistance * followDistance)
        {
            m_navigation.SetTarget(s.Owner);
        }
        else
        {
            m_navigation.Stop();
        }
    }
    public void PhysicsUpdate() { }
    public void Exit()
    {
        m_navigation.Stop();
    }
}
