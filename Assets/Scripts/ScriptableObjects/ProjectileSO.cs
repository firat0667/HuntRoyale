using Combat;
using UnityEngine;

namespace Combat.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Combat/Projectile")]
    public class ProjectileSO : ScriptableObject
    {
        [Header("Pool")]
        public int projectileID;

        [Header("Movement")]
        public float speed;
        public float range;

        [Header("Hit")]
        public int maxTargets;
    }
}
