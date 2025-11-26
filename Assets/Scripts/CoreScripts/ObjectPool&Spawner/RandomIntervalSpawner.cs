using Firat0667.CaseLib.Pattern.Pool;
using Firat0667.CaseLib.Diagnostics;
using UnityEngine;

namespace Firat0667.CaseLib.Game
{
    /// <summary>
    /// Spawner that spawns a single object at random intervals between 15 and 30 seconds.
    /// </summary>
    public class RandomIntervalSpawner : Spawner
    {
        [SerializeField] private float minSpawnInterval = 15f;
        [SerializeField] private float maxSpawnInterval = 30f;

        private float _nextSpawnTime;

        private void Awake()
        {
            _unityStopwatch.StartClock();
            SetNextSpawnTime();
        }

        private void Update()
        {
            _unityStopwatch.Tick();

            // Check if it's time to spawn the next object
            if (_unityStopwatch.Time >= _nextSpawnTime)
            {
                Spawn();

                // Reset stopwatch and set a new spawn time
                _unityStopwatch.RestartClock();
                SetNextSpawnTime();
            }
        }

        private void SetNextSpawnTime()
        {
            // Set the next spawn time to a random value between minSpawnInterval and maxSpawnInterval
            _nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
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
