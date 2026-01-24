using UnityEngine;

public class Enemy : BaseEntity
{
    public EnemyIdleState IdleState { get; private set; }
    //public EnemyFollowState FollowState { get; private set; }
    //public EnemyAttackState AttackState { get; private set; }

  
    public MovementSubsystem Movement { get; private set; }
    public AttackSubsystem Attack { get; private set; }

    private AttackSubsystem m_attack;

    public bool IsInCombat => m_attack.IsInCombat;


    protected override void Awake()
    {
        base.Awake();

        Movement = GetSubsystem<MovementSubsystem>();
        Attack = GetSubsystem<AttackSubsystem>();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnDied()
    {
        // particle effects, sound effects, etc. can be triggered here
        // xp for who killed the enemy can be awarded here
    }
    protected override void CreateStates()
    {
        IdleState = new EnemyIdleState(this);
        //FollowState = new EnemyFollowState(this);
        //AttackState = new EnemyAttackState(this);
    }
    protected override IState GetEntryState()
    {
        return IdleState;
    }
}
