using States.PlayerStates;
using Subsystems;
using System;
using UnityEngine;

public class Player : BaseEntity
{
    #region States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    #endregion

    #region Subsystems
    private MovementSubsystem m_movement;

    private AttackSubsystem m_attack;
    #endregion
    #region Animator
    private AnimatorBridge m_animatorBridge;
    public AnimatorBridge AnimatorBridge => m_animatorBridge;
    #endregion

    #region Parameters
    public bool IsInCombat => !IsDead && m_attack.IsTargetInAttackRange && m_attack.CanAttack();
    private bool m_isDead => healthSubsystem.IsDead;

    private bool m_walkAttackEnabled = false;
    public bool AllowWalkAttack => m_walkAttackEnabled;


    #endregion


    protected override void Awake()
    {
        base.Awake();
        m_movement = GetSubsystem<MovementSubsystem>();
        m_attack = GetSubsystem<AttackSubsystem>();
        m_animatorBridge = GetComponent<AnimatorBridge>();
       
    }

    protected override void Start()
    {
        base.Start();
        Initialize();
    }
    protected override void Update()
    {
        if (m_isDead)
            return;

        base.Update(); 
        if (m_movement == null)
            return;


        AnimatorBridge.UpdateMovementAnim(
            m_movement.Velocity,IsInCombat
        );
    }

    protected override void OnDied()
    {
        base.OnDied();
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_DIED);
        m_movement.Stop();
        m_animatorBridge.TriggerDead();


    }
    protected override void CreateStates()
    {
        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        AttackState = new PlayerAttackState(this);
    }
    protected override IState GetEntryState()
    {
        return IdleState;
    }
}
