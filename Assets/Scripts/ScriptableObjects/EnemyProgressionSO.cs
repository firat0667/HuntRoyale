using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Enemy/Enemy Progression")]
public class EnemyProgressionSO : ScriptableObject
{
    [Header("Level")]
    public int baseLevel = 1;

    [Header("EXP Reward")]
    public int baseExpReward = 5;
}