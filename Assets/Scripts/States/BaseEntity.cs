using Subsystems;
using System.Linq;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    protected Subsystem[] subsystems;
    protected HealthSubsystem healthSubsystem;

    public StateMachine SM { get; protected set; }

    protected virtual void Awake()
    {
        SM = new StateMachine();
        subsystems = GetComponentsInChildren<Subsystem>();
        healthSubsystem = GetSubsystem<HealthSubsystem>();

        if (healthSubsystem != null)
            healthSubsystem.OnDied?.Connect(OnDied);
    }

    protected virtual void Start()
    {
        CreateStates();
        SM.ChangeState(GetEntryState());
    }
    protected virtual void Update()
    {
        foreach (var s in subsystems)
            s.LogicUpdate();

        SM.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        foreach (var s in subsystems)
            s.PhysicsUpdate();

        SM.PhysicsUpdate();
    }

    protected virtual void OnDisable()
    {
        if (healthSubsystem != null)
            healthSubsystem.OnDied?.Disconnect(OnDied);
    }

    protected virtual void OnDied() { }
    protected abstract void CreateStates();
    protected abstract IState GetEntryState();

    public T GetSubsystem<T>() where T : Subsystem
    {
        return subsystems.OfType<T>().FirstOrDefault();
    }
}
