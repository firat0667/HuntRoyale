using UnityEngine;
public enum BotTargetingMode
{
    ClosestOnly,    
    ClosestLowHP,    
    SmartScore     
}

[CreateAssetMenu(menuName = "AI/Bot Brain Profile")]
public class BotAIBrainProfileSO : ScriptableObject
{
    [Header("Perception")]
    public float viewRadius = 8f;
    public float attackRange = 1.5f;

    public BotTargetingMode targetingMode;

    [Range(0f, 1f)] public float awareness;      
    [Range(0f, 1f)] public float aggressiveness; 
    [Range(0f, 1f)] public float greed;          
    [Range(0f, 1f)] public float intelligence;  
    [Range(0f, 1f)] public float caution;       
}
