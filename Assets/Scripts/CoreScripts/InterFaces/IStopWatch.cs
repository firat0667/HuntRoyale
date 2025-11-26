namespace Firat0667.CaseLib.Diagnostics
{
    /// <summary>
    /// Stopwatch for time keeping purposes.
    /// </summary>
    public interface IStopWatch
    {
        int TimeInSeconds { get; }
        int FixedTimeInSeconds { get; }

        void Tick();
        void FixedTick();
        void StopClock();
        void StartClock();
        void RestartClock();
    }
}
