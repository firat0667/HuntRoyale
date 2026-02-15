using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Summon")]
public class SummonSO : ScriptableObject
{
    [Header("Pool")]
    public int summonID;


    [Header("Explosion")]
    public float explosionRadius;
    public float explosionTriggerDistance;

    [Header("Spawn")]
    public int spawnCount = 1;
    public float spawnRange;
}