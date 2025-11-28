using Firat0667.CaseLib.Key;
using UnityEngine;

public class HealthSubsystem : Subsystem,IDamageable,IHealth
{
    public BasicSignal OnDied;

    private HealthCore m_core;
    public int CurrentHP => m_core.CurrentHP;
    public int MaxHP => m_core.MaxHP;

    void Start()
    {
        OnDied = new BasicSignal();
        GetCoreComponent(ref m_core);
        m_core.OnDeath.Connect(HandleDeath);

    }

    public void Damage(int amount)
    {
        m_core.ApplyDamage(amount);
    }
    private void HandleDeath()
    {
        OnDied.Emit();
    }
}
