using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, LevelComplete, GameOver }

    public event Action<GameState> OnGameStateChanged;
    public event Action<int> OnLivesChanged;

    public GameState State { get; private set; }
    public int Lives { get; private set; }
    // public int CurrentLevelIndex { get ; private set; }     
    public int TotalLevels => TOTAL_LEVELS;

    private const int STARTING_LIVES = 3;
    private const int TOTAL_LEVELS = 5;

    [Header("Debug")]
    public int CurrentLevelIndex;     

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        Lives = STARTING_LIVES;
        DontDestroyOnLoad(gameObject);
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
