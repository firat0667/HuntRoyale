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
    protected override void Update()
    {
        base.Update();

        if (m_movement == null) return;

        float speed = m_movement.Velocity.magnitude;
        float speed01 =
            speed < 0.05f ? 0f : speed / m_movement.MaxSpeed;

        Animator.SetSpeed(Mathf.Clamp01(speed01));
    }

    private void Start()
    {
        IdleState = new BotIdleState(this);
        ChaseState = new BotChaseState(this);
        AttackState = new BotAttackState(this);
        HealState = new BotHealState(this);
        SM.ChangeState(IdleState);
    }

    protected override void OnDied()
    {
        Animator.TriggerDead();
        Input.SetMove(Vector3.zero);
        Input.SetAttack(false);
        Brain.ClearTarget();
    }
}
