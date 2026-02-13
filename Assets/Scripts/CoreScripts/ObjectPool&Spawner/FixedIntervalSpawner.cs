using Firat0667.WesternRoyaleLib.Pattern.Pool;
using Firat0667.WesternRoyaleLib.Diagnostics;
using UnityEngine;

namespace Firat0667.WesternRoyaleLib.Game
{
    /// <summary>
    /// Spawner that spawns a single object every fixed interval (e.g., every 15 seconds).
    /// </summary>
    public class FixedIntervalSpawner : Spawner
    {
        [SerializeField] private float spawnInterval = 15f; // Set interval to 15 seconds
        private float _elapsedTime;

        private void Awake()
        {
            _unityStopwatch.StartClock();
        }

        private void Update()
        {
            _unityStopwatch.Tick();

            // Increment elapsed time by deltaTime
            _elapsedTime += Time.deltaTime;

            // Check if elapsed time has reached the spawn interval
            if (_elapsedTime >= spawnInterval)
            {
                Spawn();

                // Reset elapsed time after spawning
                _elapsedTime = 0f;
            }
        }

        private void OnEnable()
        {
            _unityStopwatch.Enable();
        }

        private void OnDisable()
        {
            _unityStopwatch.Disable();
        }
    }
}
