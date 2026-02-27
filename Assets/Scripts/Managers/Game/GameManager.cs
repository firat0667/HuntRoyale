using Firat0667.WesternRoyaleLib.Patterns;
using UnityEngine;

namespace Managers.Game
{
    public class GameManager : FoundationSingleton<GameManager>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        private int m_startGold = 0;
        private int m_currentGold;

        public int CurrentGold => m_currentGold;

        public Transform HealZone { get; private set; }

        private string KEY_GOLD => PlayerPrefsTag.Gold_Prefs;

        private void Awake()
        {
            GameObject zone = GameObject.FindGameObjectWithTag(Tags.HealZone_Tag);
            if (zone != null)
                HealZone = zone.transform;

            LoadGold(); 
        }

        public void AddGold(int amount)
        {
            if (amount <= 0) return;

            m_currentGold += amount;
            SaveGold();
        }

        public bool TrySpendGold(int amount)
        {
            if (amount <= 0) return true;
            if (m_currentGold < amount) return false;

            m_currentGold -= amount;
            SaveGold();
            return true;
        }

        public void SaveGold()
        {
            PlayerPrefs.SetInt(KEY_GOLD, m_currentGold);
            PlayerPrefs.Save();
        }

        public void LoadGold()
        {
            m_currentGold = PlayerPrefs.GetInt(KEY_GOLD, m_startGold);
        }
        public void ResetGold()
        {
            m_currentGold = m_startGold;
            PlayerPrefs.DeleteKey(KEY_GOLD);
            PlayerPrefs.Save();
        }
    }
}