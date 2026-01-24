using System.Xml;
using UnityEngine;

public class Agent : BaseEntity
{
    public BotInputProvider Input { get; private set; }
    public BotBrain Brain { get; private set; }
    public AnimatorBridge Animator { get; private set; }

    private MovementSubsystem m_movement;

    #region States
    public BotIdleState IdleState { get; private set; }
    public BotChaseState ChaseState { get; private set; }
    public BotAttackState AttackState { get; private set; }
    public BotHealState HealState { get; private set; }
    #endregion
  
  
    protected override void Awake()
    {
        base.Awake();
        Input = GetComponent<BotInputProvider>();
        Brain = GetComponent<BotBrain>();
        Animator = GetComponentInChildren<AnimatorBridge>();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        if (m_movement == null) return;

        Animator.SetSpeed(m_movement.Speed01);
    }
    protected override void OnDied()
    {
        Animator.TriggerDead();
        Input.SetMove(Vector3.zero);
        Input.SetAttack(false);
        Brain.ClearTarget();
    }
    protected override void CreateStates()
    {
        IdleState = new BotIdleState(this);
        ChaseState = new BotChaseState(this);
        AttackState = new BotAttackState(this);
        HealState = new BotHealState(this);
    }

    protected override IState GetEntryState()
    {
        return IdleState;
    }
}
