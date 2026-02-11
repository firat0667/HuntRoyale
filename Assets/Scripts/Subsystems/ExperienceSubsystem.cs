using Subsystems.CoreComponents;
using UnityEngine;
namespace Subsystems
{
    public class ExperienceSubsystem : Subsystem
    {
         private int baseExp = 10;

        [Header("EXP Curve")]
        [SerializeField]
        private AnimationCurve m_expCurve =
            AnimationCurve.EaseInOut(1, 1, 50, 4);

        public ExperienceCore Core { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Core = new ExperienceCore(
                baseExp,
                level => m_expCurve.Evaluate(level)
            );
        }
    }
}
    
