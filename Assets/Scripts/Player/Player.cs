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
    #endregion


    #region Animator
    private AnimatorBridge m_animatorBridge;
    #endregion
    #region Parameters

    #endregion

    public AnimatorBridge AnimatorBridge => m_animatorBridge;
  

    protected override void Awake()
    {
        base.Awake();
        m_movement = GetSubsystem<MovementSubsystem>();
        m_animatorBridge= GetComponent<AnimatorBridge>();
    }

    protected void Start()
    {
        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        AttackState = new PlayerAttackState(this);

        SM.ChangeState(IdleState);
    }
    protected override void Update()
    {
        base.Update(); 
        if (m_movement == null)
            return;
        //AnimatorBridge.UpdateMovementAnim(
        //    m_movement.Velocity
        //);
    }


    protected override void OnDied()
    {
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_DIED);
    }
}
