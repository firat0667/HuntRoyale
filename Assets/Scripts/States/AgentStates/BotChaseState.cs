using UnityEngine;

public class BotChaseState : IState
{
    private Agent m_agent;

    public BotChaseState(Agent agent)
    {
        m_agent = agent;
    }
    public void Enter()
    {
        m_agent.Input.SetAttack(false);
    }

    public void Exit()
    {
    }

    public void LogicUpdate()
    {
        //if (!_agent.Brain.HasTarget)
        //{
        //    _agent.SM.ChangeState(_agent.IdleState);
        //    return;
        //}

        //Vector3 dir = _agent.Brain.DirectionToTarget;
        //_agent.Input.SetMove(dir);
        //_agent.Input.SetAim(dir);

        //if (_agent.Brain.InAttackRange)
        //{
        //    _agent.SM.ChangeState(_agent.AttackState);
        //}
    }

    public void PhysicsUpdate()
    {
    }
}
