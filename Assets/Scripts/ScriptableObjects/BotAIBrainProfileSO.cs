using UnityEngine;

namespace AI.ScriptableObjects
{
    public enum BotTargetingMode
    {
        ClosestOnly,
        ClosestLowHP,
        SmartScore
    }

    [CreateAssetMenu(menuName = "AI/Bot Brain Profile")]
    public class BotAIBrainProfileSO : ScriptableObject
    {
        public BotTargetingMode targetingMode;

        [Range(0f, 1f)] public float awareness;
        [Range(0f, 1f)] public float aggressiveness;
        [Range(0f, 1f)] public float greed;
        [Range(0f, 1f)] public float intelligence;
        [Range(0f, 1f)] public float caution;
    }
}
