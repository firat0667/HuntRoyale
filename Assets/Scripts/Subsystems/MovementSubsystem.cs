using UnityEngine;

public class MovementSubsystem : Subsystem
{
    private MovementInputCore m_inputCore;
    private MovementPhysicsCore m_physicsCore;

    private void Start()
    {
        GetCoreComponent(ref m_inputCore);
        GetCoreComponent(ref m_physicsCore);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (m_inputCore == null || m_physicsCore == null)
            return;
        m_physicsCore.Move(m_inputCore.InputVector);
    }
}
