using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Summon")]
public class SummonSO : ScriptableObject
{
    [Header("Pool")]
    public int summonID;


    [Header("Explosion")]
    public float explosionRadius;
    public float explosionTriggerDistance;

    [Header("Attack")]
    public float attackRange;


    [Header("Spawn")]
    public int spawnCount = 1;
}