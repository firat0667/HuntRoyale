using Combat;
using UnityEngine;


[CreateAssetMenu(menuName = "Combat/Projectile")]
public class ProjectileSO : ScriptableObject
{
    [Header("Pool")]
    public int ProjectileID;

    [Header("Movement")]
    public float Speed;
    public float Range;

    [Header("Hit")]
    public int MaxTargets;
}
