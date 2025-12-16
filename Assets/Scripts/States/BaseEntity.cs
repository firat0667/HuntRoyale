using System.Linq;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    protected Subsystem[] _subsystems;
    protected HealthSubsystem _healthSubsystem;

    public StateMachine SM { get; protected set; }

    protected virtual void Awake()
    {
        SM = new StateMachine();
        _subsystems = GetComponentsInChildren<Subsystem>();
        _healthSubsystem = GetSubsystem<HealthSubsystem>();

        if (_healthSubsystem != null)
            _healthSubsystem.OnDied?.Connect(OnDied);
    }

    protected virtual void Update()
    {
        foreach (var s in _subsystems)
            s.LogicUpdate();

        SM.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        SM.PhysicsUpdate();
    }

    protected virtual void OnDisable()
    {
        if (_healthSubsystem != null)
            _healthSubsystem.OnDied?.Disconnect(OnDied);
    }

    protected virtual void OnDied() { }

    public T GetSubsystem<T>() where T : Subsystem
    {
        return _subsystems.OfType<T>().FirstOrDefault();
    }
}
