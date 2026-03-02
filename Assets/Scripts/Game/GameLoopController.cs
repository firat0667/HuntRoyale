using AI.Agents;
using CoreScripts.ObjectPool.Spawner;
using Firat0667.WesternRoyaleLib.Diagnostics;
using Firat0667.WesternRoyaleLib.Game;
using Game;
using Helper.MatchResults;
using Managers.Camera;
using Managers.Leaderboard;
using Managers.Score;
using Managers.UI;
using System.Collections.Generic;
using UI.Game;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{

    [Header("Player Settings")]

     private GameObject m_playerPrefab;
    [SerializeField] private Transform m_playerSpawnPoint;

    [Header("Match Settings")]
    [SerializeField] private float m_matchDuration = 150f;
    [SerializeField] private bool m_autoStartMatch = true;

    private float m_matchDefaultDuration;

    private bool m_isMatchActive = false;

    private GameObject m_playerInstance;

    private readonly List<EnemySpawnArea> m_spawners = new List<EnemySpawnArea>();
    private AgentSpawner m_agentSpawner;
    private MatchResultService m_resultService = new MatchResultService();
    public MatchResult LastMatchResult { get; private set; }


    private void Awake()
    {
        // save game loop controller reference
        GameRegistry.Instance.Register(KeyTags.KEY_GAME_LOOP_CONTROLLER, this);
        m_agentSpawner=FindAnyObjectByType<AgentSpawner>();
        m_agentSpawner.gameObject.SetActive(false);
        // add all spawners in the scene to the list
        m_spawners.AddRange(FindObjectsOfType<EnemySpawnArea>(true));

        m_matchDefaultDuration = m_matchDuration;

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

    void Update()
    {
        if (!m_isMatchActive)
            return;

    }
    public void SetPlayerPrefab(GameObject prefab)
    {
        m_playerPrefab = prefab;
    }
    public void StartMatch()
    {
        ScoreManager.Instance.ResetAll();
        GameStateManager.Instance.SetState(GameState.Playing);
        m_agentSpawner.gameObject.SetActive(true);
        SpawnPlayer();
        ActiveSpawners();

        m_isMatchActive = true;
        // TODO: HudManager cant be accessed here because of circular dependency, need to find a way to decouple them
        HUDManager.Instance.GameTimer.StartTimer(m_matchDuration);
        EventManager.Instance.Trigger(EventTags.EVENT_GAME_STARTED);

    }
    public void PauseGame()
    {
        if(!m_isMatchActive)
            return;
        GameStateManager.Instance.SetState(GameState.Paused);
        m_isMatchActive = false;
    }
    public void ResumeGame()
    {
        GameStateManager.Instance.SetState(GameState.Playing);
        m_isMatchActive = true;
    }
    public void RestartGame()
    {
        m_isMatchActive = false;

        if (m_playerInstance != null)
            Destroy(m_playerInstance);

 

        ScoreManager.Instance.ResetAll();
        LeaderboardManager.Instance.GetParticipants().Clear();
        SpawnPlayer();
        ActiveSpawners();
        GameStateManager.Instance.SetState(GameState.Playing);
        m_agentSpawner.gameObject.SetActive(true);

   
        m_isMatchActive = true;
        ClearAllAgents();
        m_agentSpawner.SpawnBotsManually();
        HUDManager.Instance.GameTimer.StartTimer(m_matchDefaultDuration);
        EventManager.Instance.Trigger(EventTags.EVENT_GAME_STARTED); 
    }
    private void ClearAllAgents()
    {
        var agents = FindObjectsOfType<Agent>();

        foreach (var agent in agents)
        {
            Destroy(agent.gameObject);
        }
    }

    private void SpawnPlayer()
    {
       if(!m_playerPrefab || !m_playerSpawnPoint)
        {
            Debug.LogError("[GameLoopController] Player Prefab or Spawn Point not assigned.");
            return;
        }
        m_playerInstance = Instantiate(m_playerPrefab, m_playerSpawnPoint.position, Quaternion.identity);
        m_playerInstance.name = NameTags.Player_Name;
        GameRegistry.Instance.Register(KeyTags.KEY_PLAYER, m_playerInstance);
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_SPAWNED, m_playerInstance.GetComponent<Player>());
        CameraController.Instance.SetCamera(m_playerInstance.transform);
        LeaderboardManager.Instance.RegisterParticipant(m_playerInstance.GetComponent<BaseEntity>());
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
    private void OnPlayerDied(object _)
    {
        GameLose();
    }
    public void GameWin()
    {
        if (!m_isMatchActive) return; 

        m_isMatchActive = false;
        var playerEntity = m_playerInstance.GetComponent<BaseEntity>();
        MatchResult result = m_resultService.Evaluate(playerEntity);
        LastMatchResult = result;
        EventManager.Instance.Trigger(EventTags.EVENT_MATCH_RESULT, result);
        GameStateManager.Instance.SetState(GameState.Finished);
        HUDManager.Instance.GameTimer.StopTimer();
        DeactiveAllSpawners();
        EventManager.Instance.Trigger(EventTags.EVENT_GAME_WIN);
    }
    public void GameLose()
    {
        if (!m_isMatchActive) return;

        m_isMatchActive = false;

        var playerEntity = m_playerInstance.GetComponent<BaseEntity>();
        MatchResult result = m_resultService.Evaluate(playerEntity);
        LastMatchResult = result;
        EventManager.Instance.Trigger(EventTags.EVENT_MATCH_RESULT, result);

        GameStateManager.Instance.SetState(GameState.GameLose);
        HUDManager.Instance.GameTimer.StopTimer();
        DeactiveAllSpawners();

        if (m_playerInstance != null)
            m_playerInstance.SetActive(false);
    }


}
