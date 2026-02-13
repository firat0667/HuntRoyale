using Firat0667.WesternRoyaleLib.Diagnostics;
using UnityEngine;

namespace Firat0667.WesternRoyaleLib.Diagnostics
{

    /// <summary>
    /// Manual Stopwatch to follow Unity time scale. Does not self-update, should update where initialized.
    /// </summary>
    public class ManualStopwatch : IStopWatch
    {
        public int TimeInSeconds => (int)_time;
        public float TimeInMillis => _time * 1000;
        public float Time => _time;
        private float _time;

        public int FixedTimeInSeconds => (int)_fixedTime;
        private float _fixedTime;

        private bool _running;

        /// <summary> Should Tick in Update. </summary>
        public void Tick()
        {
            if (!_running) { return; }

            _time += UnityEngine.Time.deltaTime;
        }

        /// <summary> Should Tick in FixedUpdate. </summary>
        public void FixedTick()
        {
            if (!_running) { return; }

            _fixedTime += UnityEngine.Time.fixedDeltaTime;
        }

        public void StopClock()
        {
            _running = false;
        }

        public void StartClock()
        {
            _running = true;
        }

        public void RestartClock()
        {
            _time = 0;
            _running = true;
        }

        public void Enable()
        {
            _running = true;
        }

        public void Disable()
        {
            _running = false;
        }
    }
}
