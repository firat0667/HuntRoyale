using UnityEngine;

public class AttackSubsystem : Subsystem
{
    private AttackCore m_core;
    void Start()
    {
        GetCoreComponent(ref m_core);
    }

    public void TryAttack()
    {
        m_core.Attack();
    }
}
