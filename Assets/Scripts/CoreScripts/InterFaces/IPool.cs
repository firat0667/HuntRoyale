using UnityEngine;

namespace Firat0667.WesternRoyaleLib.Behavior.GameSystem
{
    /// <summary>
    /// DI for all pool.
    /// </summary>
    public interface IPool<T> where T : MonoBehaviour
    {
        void Init(ComponentPool<T> pool);
    }
}