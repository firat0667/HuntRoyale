using Firat0667.CaseLib.Key;
using UnityEngine;

public class HealthCore : CoreComponent
{
    public BasicSignal OnDeath;

    [SerializeField] private int m_maxHealth = 100;
    private int m_currentHealth;

    public int CurrentHP => m_currentHealth;
    public int MaxHP => m_maxHealth;

    protected override void Awake()
    {
        base.Awake();   
        OnDeath = new BasicSignal();
    }
    void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    public void ApplyDamage(int amount)
    {
        m_currentHealth -= amount;

        if (m_currentHealth <= 0)
        {
            m_currentHealth = 0;
            OnDeath.Emit();
        }
    }
}
