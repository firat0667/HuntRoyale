using Pathfinding;
using States.EnemyStates;
using Subsystems;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy : BaseEntity
{

    #region States
    public EnemyIdleState IdleState { get; private set; }
    public EnemyFollowState FollowState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    #endregion

    #region Subsystems
    public MovementSubsystem Movement { get; private set; }
    public AttackSubsystem Attack { get; private set; }

    #endregion

    #region Animations
    private AnimatorBridge m_animatorBridge;
    public AnimatorBridge AnimatorBridge => m_animatorBridge;
    #endregion


    #region Properties
    public bool IsInCombat => Attack.IsTargetInAttackRange;
    public bool HasTarget => Attack.CurrentTarget != null;
    public bool IsTargetInDetectRange =>
    HasTarget &&
    Attack.Perception.CurrentTargetSqrDistance <=
    Attack.DetectRange * Attack.DetectRange;
    public bool IsTargetInAttackRange =>
    Attack.IsTargetInAttackRange;
    #endregion


    protected override void Awake()
    {
        base.Awake();

        Movement = GetSubsystem<MovementSubsystem>();
        Attack = GetSubsystem<AttackSubsystem>();
        m_animatorBridge = GetComponent<AnimatorBridge>();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        AnimatorBridge.UpdateMovementAnim(
         Movement.Velocity, IsInCombat
     );
    }
    protected override void OnDied()
    {
        // particle effects, sound effects, etc. can be triggered here
        // xp for who killed the enemy can be awarded here
    }
    protected override void CreateStates()
    {
        IdleState = new EnemyIdleState(this);
        FollowState = new EnemyFollowState(this);
        AttackState = new EnemyAttackState(this);

    }
    protected override IState GetEntryState()
    {
        return IdleState;
    }
}
