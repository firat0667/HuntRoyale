using Subsystems.CoreComponents;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Subsystems
{
    public class AimSubsystem : Subsystem
    {
        private AimCore m_core;

        public Vector2 AimDirection => m_core.AimDirection;

        protected override void Awake()
        {
            base.Awake();
            GetCoreComponent(ref m_core);
        }

        public void SetAim(Vector3 dir)
        {
            m_core.SetAim(dir);
        }
        public override void LogicUpdate()
        {
            Vector3 dir = m_core.AimDirection;
            if (dir.sqrMagnitude < 0.01f)
                return;
        }
    }
}
