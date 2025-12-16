public class Agent : BaseEntity
{
    public BotInputProvider Input { get; private set; }
    public BotBrain Brain { get; private set; }
    public BotIdleState IdleState { get; private set; }
    public BotChaseState ChaseState { get; private set; }
    public BotAttackState AttackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Input = GetComponent<BotInputProvider>();
        Brain = GetComponent<BotBrain>();
    }

    private void Start()
    {
        IdleState = new BotIdleState(this);
        ChaseState = new BotChaseState(this);
        AttackState = new BotAttackState(this);

        SM.ChangeState(IdleState);
    }

    protected override void OnDied()
    {
        // Respawn, score, drop vs.
    }
}
