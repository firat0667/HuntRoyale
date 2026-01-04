using UnityEngine;

public class MovementSubsystem : Subsystem
{
    private MovementInputCore m_inputCore;
    private MovementPhysicsCore m_physicsCore;
    private StatsComponent m_stats;

    public Vector3 Velocity => m_physicsCore != null ? m_physicsCore.Velocity : Vector3.zero;
    public float MaxSpeed => m_stats != null ? m_stats.MoveSpeed : 0f;

    private void Start()
    {
        GetCoreComponent(ref m_inputCore);
        GetCoreComponent(ref m_physicsCore);
        m_stats = GetComponentInParent<StatsComponent>();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (m_inputCore == null || m_physicsCore == null || m_stats == null)
            return;

        m_physicsCore.Move(m_inputCore.InputVector, m_stats.MoveSpeed);
    }
}
