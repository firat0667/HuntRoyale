using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Firat0667.WesternRoyaleLib.Game
{
    /// <summary>
    /// Spawner timetable data. Holds when and how many to spawn.
    /// </summary>
    [CreateAssetMenu(fileName = "New SpawnerTimetable", menuName = "FiratGames/Game/SpawnerTimetable")]
    public class SpawnerTimetable : ScriptableObject, IInitializable
    {
        [System.Serializable]
        public struct Time
        {
            public int When;
            public int HowMany;
        }

        public bool EndReached { get; private set; } = false;
        public Time[] Timeline;

        private int _iterator = 0;

        /// <summary> When is the next spawning? </summary>
        public int When() { return EndReached ? 0 : Timeline[_iterator].When; }

        /// <summary> How many will be spawning? </summary>
        public int HowMany() { return EndReached ? 0 : Timeline[_iterator].HowMany; }

        public void Next()
        {
            if (EndReached) { return; }

            _iterator++;
            EndReached = _iterator >= Timeline.Count();
        }

        public void Initialize()
        {
            _iterator = 0;
            EndReached = false;
        }
    }
}