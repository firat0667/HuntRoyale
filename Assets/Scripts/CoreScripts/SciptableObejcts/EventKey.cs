using Firat0667.WesternRoyaleLib.Key;
using UnityEngine;

namespace FiratGames.WesternRoyale.Event
{
    /// <summary>
    /// Key definition for Events.
    /// </summary>
    [CreateAssetMenu(fileName = "New EventKey", menuName = "BiscuitGames/Event/EventKey")]
    public class EventKey : ScriptableObject
    {
        public GameKey Key
        {
            get
            {
                if (_key == null)
                {
                    _key = new(name);
                }
                return _key;
            }
        }
        private GameKey _key;

        public string StringKey//TODO redundant but in use, use Key.ValueAsString from now on.
        {
            get
            {
                return name;
            }
        }
    }
}