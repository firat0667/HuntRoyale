using System;
using UnityEngine;
using Firat0667.CaseLib.Patterns;
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    GameLose,
    GameWin
}

public class GameStateManager : FoundationSingleton<GameStateManager>, IFoundationSingleton
{
    private GameState _currentState;

    public bool Initialized { get ;  set; }

    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        _currentState = GameState.MainMenu;
    }

    /// <summary>
    /// Sets a new game state.
    /// </summary>
    public void SetState(GameState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        Debug.Log($"[GameStateManager] State changed to: {newState}");

        OnStateChanged?.Invoke(newState);
        HandleStateChange(newState);
    }

    /// <summary>
    /// Returns the current game state.
    /// </summary>
    public GameState GetCurrentState()
    {
        return _currentState;
    }

    /// <summary>
    /// Handles state changes.
    /// </summary>
    private void HandleStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                Debug.Log("[GameStateManager] MainMenu state activated.");
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                Debug.Log("[GameStateManager] Game is now Playing.");
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                Debug.Log("[GameStateManager] Game is Paused.");
                break;
            case GameState.GameOver:
                Time.timeScale = 1f;
                Debug.Log("[GameStateManager] Game Over!");
                break;
        }
    }
}

/*
 * 
 * 
 * 
 * 
 * 
 * 
GameStateManager.Instance.SetState(GameState.Playing);  // change state 


if (GameStateManager.Instance.GetCurrentState() == GameState.Paused)   // control to current state 
{
    Debug.Log("Game is currently paused.");
}


// listen to  change  game state 

void Start()
{
    GameStateManager.Instance.OnStateChanged += HandleGameStateChange;
}

void HandleGameStateChange(GameState newState)
{
    Debug.Log($"Game state changed to: {newState}");
}

GameStateManager.Instance.SetState(GameState.Paused);
GameStateManager.Instance.SetState(GameState.Playing);


 * 
 * 
 * 
 * 
 * 
 * 
 */
