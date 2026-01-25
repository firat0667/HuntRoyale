using UnityEngine;

namespace States.AgentStates
{
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
            if (m_agent.Brain.ShouldHeal)
            {
                m_agent.SM.ChangeState(m_agent.HealState);
                return;
            }

            if (!m_agent.Brain.HasTarget)
            {
                m_agent.SM.ChangeState(m_agent.IdleState);
                return;
            }

            Vector3 dir = m_agent.Brain.DirectionToTarget;
            m_agent.Input.SetMove(dir);
            m_agent.Input.SetAim(dir);

            if (m_agent.Brain.InAttackRange)
            {
                m_agent.SM.ChangeState(m_agent.AttackState);
            }
        }
        public void PhysicsUpdate()
        {
        }
    }
}
