using Firat0667.WesternRoyaleLib.Game;
using Firat0667.WesternRoyaleLib.Key;
using TMPro;
using UnityEngine;


namespace UI.Game
{
    public class GameTimer : MonoBehaviour
    {
        private float m_remaining;
        private bool m_running;

        public BasicSignal<string> OnTimeUpdated = new();
        public BasicSignal OnTimerFinished = new();
        private void Awake()
        {
            OnTimerFinished.Connect(OnFinished);
        }
        private void OnDisable()
        {
            OnTimerFinished.Disconnect(OnFinished);
        }
        public void StartTimer(float duration)
        {
            m_remaining = duration;
            m_running = true;
        }
        private void OnFinished()
        {
            var loop = GameRegistry.Instance.Get<GameLoopController>(KeyTags.KEY_GAME_LOOP_CONTROLLER);
            loop.GameWin();
        }
        public void StopTimer()
        {
            m_running = false;
        }
        public void ResumeTimer()
        {
            m_running = true;
        }
        private void Update()
        {
            if (!m_running) return;

            m_remaining -= Time.deltaTime;
            m_remaining = Mathf.Max(0, m_remaining);

            OnTimeUpdated.Emit(FormatTime(m_remaining));

            if (m_remaining <= 0f)
            {
                m_running = false;
                OnTimerFinished.Emit();
            }
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            return $"{minutes}:{seconds:00}";
        }

    }
}