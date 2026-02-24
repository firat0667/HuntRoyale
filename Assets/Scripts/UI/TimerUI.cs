using Managers.UI;
using TMPro;
using UI.Game;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;
    private GameTimer m_gameTimer;
    public void Start()
    {
        m_gameTimer=GetComponent<GameTimer>();
        m_gameTimer.OnTimeUpdated.Connect(UpdateTimerText);
    }
    private void OnDisable()
    {
        m_gameTimer.OnTimeUpdated.Disconnect(UpdateTimerText);
    }
    private void UpdateTimerText(string text)
    {
        m_timerText.text = text;
    }
}
