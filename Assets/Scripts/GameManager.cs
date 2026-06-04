using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, LevelComplete, GameOver }

    public event Action<GameState> OnGameStateChanged;
    public event Action<int> OnLivesChanged;

    public GameState State { get; private set; }
    public int Lives { get; private set; }
    public int CurrentLevelIndex { get; private set; }
    public int TotalLevels => TOTAL_LEVELS;

    private const int STARTING_LIVES = 3;
    private const int TOTAL_LEVELS = 5;

    private BrickManager _brickManager;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        Lives = STARTING_LIVES;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        UnsubscribeBrickManager();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UnsubscribeBrickManager();
        _brickManager = BrickManager.Instance;
        if (_brickManager != null)
            _brickManager.LevelCleared += OnLevelComplete;
    }

    private void OnSceneUnloaded(Scene scene) => UnsubscribeBrickManager();

    /// <summary>
    /// BrickManager is the singleton.
    /// You should reference to it like this: BrickManager.Instance.Do();
    /// Why did you create variable reference to it.
    /// </summary>
    private void UnsubscribeBrickManager()
    {
        if (_brickManager != null)
        {
            _brickManager.LevelCleared -= OnLevelComplete;
            _brickManager = null;
        }
    }

    public void StartGame()
    {
        Lives = STARTING_LIVES;
        CurrentLevelIndex = 0;
        ScoreManager.Instance?.Reset();
        SetState(GameState.Playing);
        SceneLoader.Load(SceneLoader.GAME);
    }

    public void OnBallLost()
    {
        Lives--;
        OnLivesChanged?.Invoke(Lives);
        if (Lives <= 0) TriggerGameOver();
    }

    public void OnLevelComplete()
    {
        CurrentLevelIndex++;
        if (CurrentLevelIndex >= TOTAL_LEVELS)
        {
            TriggerGameOver();
            return;
        }
        // Listeners (BrickManager) reload the grid synchronously on this event,
        // then we resume play so per-ball logic gated on Playing keeps working.
        SetState(GameState.LevelComplete);
        SetState(GameState.Playing);
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void ReturnToMainMenu()
    {
        SetState(GameState.MainMenu);
        SceneLoader.Load(SceneLoader.MAIN_MENU);
    }

    private void TriggerGameOver()
    {
        SetState(GameState.GameOver);
        SceneLoader.Load(SceneLoader.GAME_OVER);
    }

    private void SetState(GameState newState)
    {
        State = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}
