using Firat0667.CaseLib.Game;
using UnityEngine;

namespace Firat0667.CaseLib.Game
{
    /// <summary>
    /// Spawner implementation that spawns on a timetable.
    /// </summary>
    public class TimedSpawner : BatchSpawner
    {
        [SerializeField] private SpawnerTimetable _timeTable;

        private void Awake()
        {
            _unityStopwatch.StartClock();

            _timeTable.Initialize();
        }

        private void Update()
        {
            if (_timeTable.EndReached)
            {
                return;
            }

            SpawnOnTime();
        }

        private void SpawnOnTime()
        {
            _unityStopwatch.Tick();

            if (_unityStopwatch.TimeInSeconds > _timeTable.When())
            {
                int spawnCount = _timeTable.HowMany();
                _timeTable.Next();

                for (int i = 0; i < spawnCount; i++)
                {
                    Spawn();
                }
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