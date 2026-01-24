using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MovementSubsystem : Subsystem
{
    [SerializeField] private Transform visualRoot;

    private MovementPhysicsCore m_physics;
    private Vector3 m_moveDir;
    private float m_speedMultiplier = 1f;

    public Vector3 Velocity => m_physics.Velocity;
    public float MaxSpeed => StatsComponent.MoveSpeed;

    public float Speed01
    {
        get
        {
            float max = Mathf.Max(StatsComponent.MoveSpeed, 0.01f);
            return Mathf.Clamp01(Velocity.magnitude / max);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        GetCoreComponent(ref m_physics);
    }

    public void SetMoveDirection(Vector3 dir)
    {
        m_moveDir = dir;
    }

    public void Stop()
    {
        m_moveDir = Vector3.zero;
    }

    public override void LogicUpdate()
    {
        float speed = StatsComponent.MoveSpeed * m_speedMultiplier;
        m_physics.Move(m_moveDir, speed);
    }

    public void RotateTowards(Vector3 dir)
    {
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion target = Quaternion.LookRotation(dir);
        float t = 1f - Mathf.Exp(-StatsComponent.RotationSpeed * Time.deltaTime);

        visualRoot.rotation = Quaternion.Slerp(visualRoot.rotation, target, t);
    }

    public void SetSpeedMultiplier(float v)
    {
        m_speedMultiplier = v;
    }
}

