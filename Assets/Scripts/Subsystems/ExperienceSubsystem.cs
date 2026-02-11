using Firat0667.CaseLib.Key;
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

        public BasicSignal<int> OnLevelUp;
        public BasicSignal<int, int> OnExpChanged;

        public int Level => Core.Level;

        protected override void Awake()
        {
            base.Awake();

            OnLevelUp = new BasicSignal<int>();
            OnExpChanged = new BasicSignal<int, int>();

            Core = new ExperienceCore(
                baseExp,
                level => m_expCurve.Evaluate(level)
            );
        }

        public void AddExp(int amount)
        {
            bool leveledUp = Core.AddExp(amount);
            OnExpChanged.Emit(Core.CurrentExp, Core.RequiredExp);

            if (leveledUp)
            {
                OnLevelUp.Emit(Core.Level);
            }
        }
    }
}
    
