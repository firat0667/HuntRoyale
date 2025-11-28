using UnityEngine;

public class PlayerAimInputCore : CoreComponent
{
    private PlayerInputCore m_input;
    private AimCore m_aim;

    private void Awake()
    {
        base.Awake();
        m_input=core.GetCoreComponent<PlayerInputCore>();
        m_aim=core.GetCoreComponent<AimCore>();
    }
    public override void LogicUpdate()
    {
        if (m_input.MoveInput.sqrMagnitude > 0.1f)
            m_aim.SetAim(m_input.MoveInput);
    }
}
