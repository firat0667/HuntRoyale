using UnityEngine;


namespace Subsystems.CoreComponents
{
    public class ExperienceCore : CoreComponent
    {
        public int Level { get; private set; }
        public int CurrentExp { get; private set; }
        public int RequiredExp { get; private set; }

        private readonly int m_baseExp;

        private readonly System.Func<int, float> m_curveEval;

        public ExperienceCore(int baseExp, System.Func<int, float> curveEval)
        {
            m_baseExp = baseExp;
            m_curveEval = curveEval;
            Reset();
        }

        public void Reset()
        {
            Level = 1;
            CurrentExp = 0;
            RequiredExp = CalcRequiredExp(Level);
        }
        public bool AddExp(int amount)
        {
            if (amount <= 0)
                return false;

            CurrentExp += amount;

            bool leveledUp = false;
            int safetyCounter = 0;

            while (CurrentExp >= RequiredExp && RequiredExp > 0)
            {
                LevelUp();
                leveledUp = true;
                safetyCounter++;
                if (safetyCounter > 50)
                {
                    Debug.LogWarning("ExperienceCore: Possible infinite level loop.");
                    break;
                }
            }

            return leveledUp;
        }
        private void LevelUp()
        {
            CurrentExp -= RequiredExp;
            Level++;
            RequiredExp = CalcRequiredExp(Level);
        }
        private int CalcRequiredExp(int level)
        {
            float curveMultiplier = m_curveEval?.Invoke(level) ?? 1f;
            return Mathf.CeilToInt(m_baseExp * level * curveMultiplier);
        }
    }
}
