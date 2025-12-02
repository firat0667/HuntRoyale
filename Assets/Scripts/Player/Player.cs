using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Subsystem[] _subsystems;

    private HealthSubsystem _healthSubsystem;

    public StateMachine SM { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }

    private void Awake()
    {

        SM = new StateMachine();

        _subsystems = GetComponentsInChildren<Subsystem>();

        _healthSubsystem = GetSubsystem<HealthSubsystem>();

        _healthSubsystem.OnDied?.Connect(OnPlayerDied);
    }
    private void Start()
    {
        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        AttackState = new PlayerAttackState(this);
        SM.ChangeState(IdleState);


    }
    private void OnDisable()
    {
       _healthSubsystem.OnDied?.Disconnect(OnPlayerDied);
    }
    private void Update()
    {
        foreach (var s in _subsystems)
            s.LogicUpdate();

        SM.LogicUpdate();
    }
    private void FixedUpdate()
    {
        SM.PhysicsUpdate();
    }

    public T GetSubsystem<T>() where T : Subsystem
    {
        return _subsystems.OfType<T>().FirstOrDefault();
    }
    private void OnPlayerDied()
    {
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_DIED);
    }
}
