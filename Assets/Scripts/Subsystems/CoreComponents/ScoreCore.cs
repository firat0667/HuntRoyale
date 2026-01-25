using UnityEngine;

namespace Subsystems.CoreComponents
{
    public class ScoreCore : CoreComponent
    {
        public int Score { get; private set; }

        public void Add(int amount)
        {
            Score += amount;
            EventManager.Instance.Trigger("ScoreChanged", Score);
        }
    }

}
