using System.Collections.Generic;
using Combat.Stats.ScriptableObjects;
using Managers.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.CharacterSelection 
{
    public class CharacterSelectionManager : MonoBehaviour
    {
        [SerializeField] private List<CharacterDataSO> m_characters;
        [SerializeField] private Transform m_previewRoot;

        #region UI
        [SerializeField] private TextMeshProUGUI m_characterNameText;
        [SerializeField] private TextMeshProUGUI m_characterStatsText;
        [SerializeField] private Button m_actionButton;
        [SerializeField] private TextMeshProUGUI m_actionButtonText;
        [SerializeField] private Image m_actionButtonImage;
        #endregion

        private const string KEY_UNLOCKED = "unlocked_characters";
        private List<int> m_unlockedIndexes = new();

        private List<GameObject> spawnedPreviews = new();
        private int currentIndex = 0;

        private void Start()
        {
            m_unlockedIndexes = SaveManager.Instance.LoadList<int>(KEY_UNLOCKED);

            if (!m_unlockedIndexes.Contains(0))
                m_unlockedIndexes.Add(0);

            SpawnAllPreviews();
            ActivateCharacter(currentIndex);
        }

        private void SpawnAllPreviews()
        {
            foreach (var data in m_characters)
            {
                GameObject obj = Instantiate(
                 data.menuPreviewPrefab,
                 m_previewRoot
             );

                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;

                obj.SetActive(false);
                spawnedPreviews.Add(obj);
            }
        }

        public void NextCharacter()
        {
            currentIndex++;
            if (currentIndex >= spawnedPreviews.Count)
                currentIndex = 0;

            ActivateCharacter(currentIndex);
        }

        public void PreviousCharacter()
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = spawnedPreviews.Count - 1;

            ActivateCharacter(currentIndex);
        }

        private void ActivateCharacter(int index)
        {
            for (int i = 0; i < spawnedPreviews.Count; i++)
            {
                spawnedPreviews[i].SetActive(i == index);
            }

            UpdateInfoPanel(m_characters[index]);
            UpdateButtonState(m_characters[index]);
        }
        public void OnActionButtonPressed()
        {
            var data = m_characters[currentIndex];
            bool isUnlocked = m_unlockedIndexes.Contains(currentIndex);

            if (isUnlocked)
            {
                SaveManager.Instance.Save("selected_character", currentIndex);
            }
            else
            {
                if (GameManager.Instance.TrySpendGold(data.unlockCost))
                {
                    m_unlockedIndexes.Add(currentIndex);
                    SaveManager.Instance.SaveList(KEY_UNLOCKED, m_unlockedIndexes);
                    SaveManager.Instance.Save("selected_character", currentIndex);
                }
            }

            ActivateCharacter(currentIndex);
        }
        private void UpdateButtonState(CharacterDataSO data)
        {
            bool isUnlocked = m_unlockedIndexes.Contains(currentIndex);
            int selectedIndex = SaveManager.Instance.Load<int>("selected_character");
            bool isSelected = selectedIndex == currentIndex;

            int gold = GameManager.Instance.CurrentGold;

            if (isUnlocked)
            {
                if (isSelected)
                {
                    m_actionButtonText.text = "SELECTED";
                    m_actionButton.interactable = false;
                    m_actionButtonImage.color = new Color(1f, 0.85f, 0.2f); 
                }
                else
                {
                    m_actionButtonText.text = "SELECT";
                    m_actionButton.interactable = true;
                    m_actionButtonImage.color = Color.white;
                }
            }
            else
            {
                m_actionButtonText.text = $"BUY ({data.unlockCost})";

                if (gold >= data.unlockCost)
                {
                    m_actionButton.interactable = true;
                    m_actionButtonImage.color = Color.white;
                }
                else
                {
                    m_actionButton.interactable = false;
                    m_actionButtonImage.color = new Color(0.3f, 0.3f, 0.3f); 
                }
            }
        }
        private void UpdateInfoPanel(CharacterDataSO data)
        {
            m_characterNameText.text = data.characterName;

            var stats = data.baseStats;

            float range = 0f;

            switch (stats.attackType)
            {
                case AttackType.Ranged:
                    range = stats.projectileStats.range;
                    break;

                case AttackType.Summon:
                    range = stats.summonStats.spawnRange;
                    break;

                default:
                    range = stats.attackStartRange;
                    break;
            }

            m_characterStatsText.text =
                $"TYPE: {stats.attackType}   " +
                $"HP: {stats.maxHP}   " +
                $"DMG: {stats.attackDamage}   " +
                $"RNG: {range}   " +
                $"SPD: {stats.moveSpeed}    ";

            foreach (var effect in stats.onHitEffects)
            {
                m_characterStatsText.text += $"EFFECT:   {effect.name}";
            }

            foreach (var effect in stats.selfEffects)
            {
                m_characterStatsText.text += $"EFFECT:  {effect.name}";
            }
        }
    }

}
