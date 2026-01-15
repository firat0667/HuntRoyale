using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MovementSubsystem : Subsystem
{
    [SerializeField] private Transform visualRoot;
    private MovementInputCore m_inputCore;
    private MovementPhysicsCore m_physicsCore;

    private float _speedMultiplier = 1f;
    private Quaternion _desiredRotation;
    private float _rotationVelocity;

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

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);

        float t = 1f - Mathf.Exp(-StatsComponent.RotationSpeed * Time.deltaTime);

        visualRoot.rotation = Quaternion.Slerp(
            visualRoot.rotation,
            targetRot,
            t
        );
    }

    public void SetSpeedMultiplier(float value)
    {
        _speedMultiplier = Mathf.Clamp(value, 0f, 2f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (m_inputCore == null || m_physicsCore == null || StatsComponent == null)
            return;

        float finalSpeed = StatsComponent.MoveSpeed * _speedMultiplier;
        m_physicsCore.Move(m_inputCore.InputVector, finalSpeed);

    }
}
