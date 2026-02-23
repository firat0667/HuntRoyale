using UnityEngine;


namespace Combat.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Combat/KillRewards")]
    public class KillRewardsSO : ScriptableObject
    {
        [Header("Kill Rewards")]
        public int scoreReward;
        public int expReward;
        public KillRewardEntry[] killRewardEntries;
    }

    [System.Serializable]
    public class KillRewardEntry
    {
        public GameObject starPrefab;
        public float spawnChance;
    }
}
