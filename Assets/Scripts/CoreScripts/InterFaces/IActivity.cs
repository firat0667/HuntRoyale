using Firat0667.CaseLib.DI;
using Firat0667.CaseLib.Key;
using UnityEngine;

namespace FiratGames.CampSimulator.Service
{
    /// <summary>
    /// DI for Activity.
    /// </summary>
    public interface IActivity
    {
        GameKey TypeKey { get; }
        bool Occupied { get; }

        void Occupy();
        void Vacate();
    }

    /// <summary>
    /// State controls for an activity.
    /// </summary>
    [CreateAssetMenu(fileName = "New Activity", menuName = "BiscuitGames/Activity")]
    public class Activity : ScriptableObject, IActivity, IInitializable
    {
        /// <summary> This key is the same for same Activity type. </summary>
        public GameKey TypeKey { get; private set; }
        /// <summary> This key is unique per activity, regardless of type. </summary>
        public LongKey UniqueKey { get; private set; }

        public bool Occupied => _occupied;
        private bool _occupied;

        public Transform Transform => _transform;
        private Transform _transform;

        public bool Initialized { get; set; }

        public void Init(Container transform, Container activityNameAsType)
        {
            Init(activityNameAsType);
            _transform = (Transform)transform.Value;
        }

        public void Init(Container activityNameAsType)
        {
            var nameAsType = (string)activityNameAsType.Value;
            TypeKey = new(nameAsType);
            UniqueKey = new(TypeKey.ValueAsString);

            _occupied = false;
        }

        public void Occupy()
        {
            _occupied = true;
        }

        public void Vacate()
        {
            _occupied = false;
        }
    }
}
