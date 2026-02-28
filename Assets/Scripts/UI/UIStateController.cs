using Game.UI;
using UnityEngine;

public class UIStateController : MonoBehaviour
{
    [Header("UI Views")]
    [SerializeField] private UIBase m_mainMenu;
    [SerializeField] private UIBase m_hud;
    [SerializeField] private UIBase m_win;
    [SerializeField] private UIBase m_lose;

    private void Start()
    {
        GameStateManager.Instance.OnStateChanged.Connect(HandleGameState);
        //HandleGameState(GameStateManager.Instance.GetCurrentState());
    }
    private void OnDisable()
    {
        GameStateManager.Instance.OnStateChanged.Disconnect(HandleGameState);
    }
    private void HandleGameState(GameState state)
    {
        m_mainMenu.Hide();
        m_hud.Hide();
        m_win.Hide();
        m_lose.Hide();

        switch (state)
        {
            case GameState.MainMenu:
                m_mainMenu.Show();
                break;

            case GameState.Playing:
                m_hud.Show();
                break;

            case GameState.Finished:
                m_win.Show();
                break;

            case GameState.GameLose:
            case GameState.GameOver:
                m_lose.Show();
                break;
        }
    }
}
