using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AimSubsystem : Subsystem
{
    private AimCore m_core;

    public Vector2 AimDirection => m_core.AimDirection;

    void Start()
    {
        GetCoreComponent(ref m_core);
    }
}
