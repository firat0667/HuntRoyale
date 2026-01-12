using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MovementSubsystem : Subsystem
{
    [SerializeField] private Transform visualRoot;
    private MovementInputCore m_inputCore;
    private MovementPhysicsCore m_physicsCore;

    public Vector3 Velocity => m_physicsCore != null ? m_physicsCore.Velocity : Vector3.zero;
    public float MaxSpeed => StatsComponent != null ? StatsComponent.MoveSpeed : 0f;

    protected override void Awake()
    {
        base.Awake();
        GetCoreComponent(ref m_inputCore);
        GetCoreComponent(ref m_physicsCore);
    }
    public void RotateTowards(Vector3 dir)
    {
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

        visualRoot.rotation = Quaternion.RotateTowards(
            visualRoot.rotation,
            rot,
            StatsComponent.RotationSpeed * Time.fixedDeltaTime
        );
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (m_inputCore == null || m_physicsCore == null || StatsComponent == null)
            return;

        m_physicsCore.Move(m_inputCore.InputVector, StatsComponent.MoveSpeed);
    }
}
