using UnityEngine;

public class AimCore : CoreComponent
{
    public Vector3 AimDirection { get; private set; } = Vector3.forward;

    public void SetAim(Vector3 dir)
    {
        if (dir.sqrMagnitude > 0.1f)
            AimDirection = dir.normalized;
    }
}
