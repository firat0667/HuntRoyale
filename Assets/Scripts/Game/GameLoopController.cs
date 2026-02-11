using CoreScripts.ObjectPool.Spawner;
using Firat0667.CaseLib.Diagnostics;
using Firat0667.CaseLib.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopController : MonoBehaviour
{

    [Header("Player Settings")]

    [SerializeField] private GameObject m_playerPrefab;
    [SerializeField] private Transform m_playerSpawnPoint;

    [Header("Match Settings")]
    [SerializeField] private float m_matchDuration = 150f;
    [SerializeField] private bool m_autoStartMatch = true;

    private ManualStopwatch m_matchTimer = new ManualStopwatch();
    private bool m_isMatchActive = false;

    private GameObject m_playerInstance;

    private readonly List<EnemySpawnArea> m_spawners = new List<EnemySpawnArea>();



    private void Awake()
    {
        // save game loop controller reference
        GameRegistry.Instance.Register(KeyTags.KEY_GAME_LOOP_CONTROLLER, this);

        // add all spawners in the scene to the list
        m_spawners.AddRange(FindObjectsOfType<EnemySpawnArea>(true));

        //when player dead trigger game over
        EventManager.Instance.Subscribe(EventTags.EVENT_PLAYER_DIED, OnPlayerDied);

        // game start state
        GameStateManager.Instance.SetState(GameState.MainMenu);
    }

    void Start()
    {
        if(m_autoStartMatch)
        {
            StartMatch();
        }
    }
    private void OnDestroy()
    {
        if(EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(EventTags.EVENT_PLAYER_DIED, OnPlayerDied);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isMatchActive)
            return;

        m_matchTimer.Tick();
        EventManager.Instance.Trigger(EventTags.EVENT_MATCH_TIME_UPDATED, m_matchTimer.Time);

        if (m_matchTimer.Time >= m_matchDuration)
        {
            GameWin();
            //EvaluateMatchResult();
        }

    }
    public void StartMatch()
    {
        GameStateManager.Instance.SetState(GameState.Playing);

        SpawnPlayer();

        ActiveSpawners();

        m_matchTimer.RestartClock();
        m_isMatchActive = true;

        EventManager.Instance.Trigger(EventTags.EVENT_GAME_STARTED);

    }
    public void PauseGame()
    {
        if(!m_isMatchActive)
            return;
        GameStateManager.Instance.SetState(GameState.Paused);
        m_isMatchActive = false;
        m_matchTimer.StopClock();
    }
    public void ResumeGame()
    {
        GameStateManager.Instance.SetState(GameState.Playing);
        m_isMatchActive = true;
        m_matchTimer.StartClock();
    }
    public void RestartGame()
    {
        GameStateManager.Instance.SetState(GameState.Playing);

        // Respawn player
        if (m_playerInstance == null)
        {
            SpawnPlayer();
        }
        else
        {
            m_playerInstance.SetActive(true);
            m_playerInstance.transform.SetPositionAndRotation(m_playerSpawnPoint.position, m_playerSpawnPoint.rotation);
        }
        // Remove All Enemies
        ClearEnemies();

        ActiveSpawners();

        m_matchTimer.RestartClock();
        m_isMatchActive = true;

        EventManager.Instance.Trigger(EventTags.EVENT_GAME_RESTARTED);
    }
    //private void EvaluateMatchResult()
    //{
    //    int myScore = ScoreManager.Instance.GetPlayerScore();
    //    int maxScore = ScoreManager.Instance.GetHighestScoreAmongAllPlayers();

    //    if (myScore >= maxScore)
    //        GameWin();
    //    else
    //        GameLose();
    //}
    private void SpawnPlayer()
    {
       if(!m_playerPrefab || !m_playerSpawnPoint)
        {
            Debug.LogError("[GameLoopController] Player Prefab or Spawn Point not assigned.");
            return;
        }
        m_playerInstance = Instantiate(m_playerPrefab, m_playerSpawnPoint.position, Quaternion.identity);
        GameRegistry.Instance.Register(KeyTags.KEY_PLAYER, m_playerInstance);
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_SPAWNED, m_playerInstance.GetComponent<Player>());
        CameraController.Instance.SetCamera(m_playerInstance.transform);
    }
    private void ActiveSpawners()
    {
        foreach(var spawner in m_spawners)
        {
            if(spawner != null)
            {
               spawner.gameObject.SetActive(true);
            }
        }
    }
    private void DeactiveAllSpawners()
    {
        foreach (var spawner in m_spawners)
        {
            if (spawner != null)
            {
                spawner.gameObject.SetActive(false);
            }
        }
    }
    private void ClearEnemies()
    {
        // TODO : Implement enemy clearing logic
        // ALL active should return to object pool or be destroyed
    }
    private void OnPlayerDied(object _)
    {
        GameLose();
    }
    public void GameWin()
    {
        if (!m_isMatchActive) return;

        m_isMatchActive = false;
        m_matchTimer.StopClock();

        GameStateManager.Instance.SetState(GameState.GameWin);

        DeactiveAllSpawners();

        EventManager.Instance.Trigger(EventTags.EVENT_GAME_WIN);
    }
    public void GameLose()
    {
        if (!m_isMatchActive) return;

        m_isMatchActive = false;
        m_matchTimer.StopClock();

        GameStateManager.Instance.SetState(GameState.GameLose);

        DeactiveAllSpawners();

        if (m_playerInstance != null)
            m_playerInstance.SetActive(false);

        EventManager.Instance.Trigger(EventTags.EVENT_GAME_LOSE);
    }


}
