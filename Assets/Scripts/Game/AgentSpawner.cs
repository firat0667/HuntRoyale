using AI.Brain;
using AI.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;


namespace Game 
{
    public class AgentSpawner : MonoBehaviour
    {
        [Header("Bot Settings")]
        [SerializeField] private List<GameObject> m_botPrefabs;
        [SerializeField] private List<BotAIBrainProfileSO> m_brainProfiles;
        [SerializeField] private int m_botCount = 3;

        [Header("Spawn Points")]
        [SerializeField] private Transform[] m_spawnPoints;

        private void Start()
        {
            SpawnBots();
        }
        private void SpawnBots()
        {
            if (m_botPrefabs.Count == 0 || m_spawnPoints.Length == 0)
            {
                Debug.LogWarning("BotSpawner: Prefabs or SpawnPoints missing.");
                return;
            }

            List<Transform> availableSpawns = new List<Transform>(m_spawnPoints);
            ShuffleList(availableSpawns);

            int spawnAmount = Mathf.Min(m_botCount, availableSpawns.Count);

            for (int i = 0; i < spawnAmount; i++)
            {
                SpawnSingleBot(availableSpawns[i]);
            }
        }
        private void SpawnSingleBot(Transform spawnPoint)
        {
            GameObject randomPrefab =
                m_botPrefabs[Random.Range(0, m_botPrefabs.Count)];

            Quaternion randomRotation =
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            GameObject bot =
                Instantiate(randomPrefab, spawnPoint.position, randomRotation);

            AssignRandomProfile(bot);
        }
        private void AssignRandomProfile(GameObject bot)
        {
            if (m_brainProfiles == null || m_brainProfiles.Count == 0)
                return;

            BotBrain brain = bot.GetComponentInChildren<BotBrain>();

            if (brain == null)
                return;

            BotAIBrainProfileSO randomProfile =
                m_brainProfiles[Random.Range(0, m_brainProfiles.Count)];

            brain.SetProfile(randomProfile);
        }
        private void ShuffleList<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}
