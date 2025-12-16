using UnityEngine;

public class BotAttackState : IState
{
    private Agent m_agent;

    public BotAttackState(Agent agent)
    {
        m_agent = agent;
    }
    public void Enter()
    {
        m_agent.Input.SetAttack(true);
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

        //if (!_agent.Brain.InAttackRange)
        //{
        //    _agent.SM.ChangeState(_agent.ChaseState);
        //}
    }


    public void PhysicsUpdate()
    {
    }
}
