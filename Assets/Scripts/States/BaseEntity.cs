using Combat.Effects;
using Combat.Effects.ScriptableObjects;
using Subsystems;
using System.Linq;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    protected Subsystem[] subsystems;

    protected HealthSubsystem healthSubsystem;
    protected EffectSubsystem effectSubsystem;

    public StateMachine SM { get; protected set; }
    public bool IsDead => healthSubsystem != null
                      && healthSubsystem.MaxHP > 0
                      && healthSubsystem.IsDead;
    public HealthSubsystem Health => healthSubsystem;


    protected virtual void OnEnable()
    {
        Initialize();
    }
  
    protected virtual void Awake()
    {
        SM = new StateMachine();
        subsystems = GetComponentsInChildren<Subsystem>();
        
    }

    protected void Initialize()
    {
        healthSubsystem = GetSubsystem<HealthSubsystem>();
        effectSubsystem = GetSubsystem<EffectSubsystem>();

        if (healthSubsystem != null)
            healthSubsystem.OnDied?.Connect(OnDied);
    }

    protected virtual void Start()
    {
        Initialize();
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
    public virtual void ApplyEffect(StatusEffect effect, StatusEffectSO source)
    {
        if (effectSubsystem == null)
            return;

        effectSubsystem.AddEffect(effect);
    }
    public virtual void OnDealDamage(int damage)
{
    if (effectSubsystem == null)
        return;

    foreach (var effect in effectSubsystem.ActiveEffects)
    {

        if (effect is ILifeStealProvider provider)
        {
            int heal = Mathf.RoundToInt(damage * provider.GetLifeStealPercent());
            Health.Heal(heal);
        }
    }
}
    protected virtual void OnDied() { }
    protected abstract void CreateStates();
    protected abstract IState GetEntryState();

    public T GetSubsystem<T>() where T : Subsystem
    {
        return subsystems.OfType<T>().FirstOrDefault();
    }
}
