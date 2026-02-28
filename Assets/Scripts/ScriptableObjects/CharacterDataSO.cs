using UnityEngine;
using Combat.Stats.ScriptableObjects;

[CreateAssetMenu(menuName = "Menu/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [Header("Identity")]
    public string characterName;

    [Header("Prefabs")]
    public GameObject gameplayPrefab;      
    public GameObject menuPreviewPrefab;   

    [Header("Stats")]
    public BaseStatsSO baseStats;

    [Header("Unlock")]
    public int unlockCost;
    public bool isDefaultUnlocked; 
}