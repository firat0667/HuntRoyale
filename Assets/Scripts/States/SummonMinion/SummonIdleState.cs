using Combat;
using Subsystems.Ai;
namespace States.SummonMinionStates
{
    public class SummonIdleState : IState
    {
        private readonly SummonMinion s;
        private readonly AINavigationSubsystem m_navigation;

        public SummonIdleState(SummonMinion summon)
        {
            s = summon;
            m_navigation=summon.Navigation;
        }

        public void Enter()
        {
            m_navigation.Stop();
        }
        public void LogicUpdate()
        {
            if (s.Owner == null)
                return;

            float ownerDist =
                (s.transform.position - s.Owner.position).sqrMagnitude;

            if (s.CurrentTarget != null || ownerDist > 4f)
            {
                s.SM.ChangeState(s.FollowState);
            }
        }
        public void PhysicsUpdate() { }

         

        public void Exit() { }
    
    }
}