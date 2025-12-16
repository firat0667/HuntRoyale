using UnityEngine;

public class BotIdleState : IState
{
    private Agent m_agent;

    public BotIdleState(Agent agent)
    {
        m_agent = agent;
    }

    public void Enter()
    {
        m_agent.Input.SetMove(Vector3.zero);
        m_agent.Input.SetAttack(false);
    }

    public void Exit()
    {
    }

    public void LogicUpdate()
    {
        //if (_agent.Brain.HasTarget)
        //{
        //    _agent.SM.ChangeState(_agent.ChaseState);
        //}
    }

    public void PhysicsUpdate()
    {
    }

}
