using UnityEngine;


namespace States.AgentStates
{
    public class BotHealState : IState
    {
        private Agent m_agent;

        public BotHealState(Agent agent)
        {
            m_agent = agent;
        }

        public void Enter()
        {
            m_agent.Input.SetAttack(false);
        }

        public void Exit()
        {
            m_agent.Input.SetMove(Vector3.zero);
        }

        public void LogicUpdate()
        {
            if (!m_agent.Brain.ShouldHeal)
            {
                m_agent.SM.ChangeState(m_agent.IdleState);
                return;
            }
            Vector3 dir = m_agent.Brain.DirectionToHealZone;
            m_agent.Input.SetMove(dir);
            m_agent.Input.SetAim(dir);
        }

        public void PhysicsUpdate()
        {
        }

    }
}
