using Firat0667.WesternRoyaleLib.Key;
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

        public BasicSignal<int> GoldChanged { get; private set; }


        private void Awake()
        {
            GameObject zone = GameObject.FindGameObjectWithTag(Tags.HealZone_Tag);
            if (zone != null)
                HealZone = zone.transform;

            GoldChanged = new BasicSignal<int>();

        }
        private void Start()
        {
            LoadGold();
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                AddGold(100);
            }
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
            GoldChanged.Emit(m_currentGold);
            SaveManager.Instance.Save(KEY_GOLD, m_currentGold);
        }

        public void LoadGold()
        {
            m_currentGold = SaveManager.Instance.Load<int>(KEY_GOLD);
            GoldChanged.Emit(m_currentGold);
        }
        public void ResetGold()
        {
            m_currentGold = m_startGold;
            PlayerPrefs.DeleteKey(KEY_GOLD);
            PlayerPrefs.Save();
        }
    }
}