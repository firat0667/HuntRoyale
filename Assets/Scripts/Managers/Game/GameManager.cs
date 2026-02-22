using Firat0667.WesternRoyaleLib.Patterns;
using UnityEngine;


namespace Managers.Game
{
    public class GameManager : FoundationSingleton<GameManager>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        public Transform HealZone { get; private set; }

        private void Awake()
        {
            GameObject zone = GameObject.FindGameObjectWithTag(Tags.HealZone_Tag);
            if (zone != null)
                HealZone = zone.transform;
        }

    }
}
