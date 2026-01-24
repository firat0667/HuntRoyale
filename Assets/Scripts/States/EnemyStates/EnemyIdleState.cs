public class EnemyIdleState : IState
{
    private readonly Enemy m_enemy;

    public EnemyIdleState(Enemy enemy)
    {
        m_enemy = enemy;
    }

    public void Enter()
    {
       // aipath .stop();
    }

    public void LogicUpdate()
    {
        if (m_enemy.Attack.CurrentTarget == null)
            return;

        //if (m_enemy.IsInCombat)
        //{
        //    m_enemy.SM.ChangeState(m_enemy.AttackState);
        //}
        //else
        //{
        //    m_enemy.SM.ChangeState(m_enemy.FollowState);
        //}
    }



    public void PhysicsUpdate() { }
    public void Exit() { }
}
