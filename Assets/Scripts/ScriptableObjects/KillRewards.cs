using UnityEngine;


namespace Combat.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Combat/KillRewards")]
    public class KillRewards : ScriptableObject
    {
        [Header("Kill Rewards")]
        public int scoreReward;
        public KillRewardEntry[] killRewardEntries;
    }

    [System.Serializable]
    public class KillRewardEntry
    {
        public GameObject starPrefab;
        public float spawnChance;
    }
}
