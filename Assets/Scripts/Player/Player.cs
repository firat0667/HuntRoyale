public class Player : BaseEntity
{
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }

    private AnimatorBridge m_animatorBridge;

    public AnimatorBridge AnimatorBridge
    {
        get
        {
            if (m_animatorBridge == null)
            {
                m_animatorBridge = GetComponent<AnimatorBridge>();
            }
            return m_animatorBridge;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        AttackState = new PlayerAttackState(this);

        SM.ChangeState(IdleState);
    }

    protected override void OnDied()
    {
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_DIED);
    }
}
