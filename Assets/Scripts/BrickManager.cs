using System;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public static BrickManager Instance { get; private set; }

    public event Action<Vector3, Color> BrickDestroyed;
    public event Action                 BrickDamaged;
    public event Action                 LevelCleared;

    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private LevelData[] _levels;
    [SerializeField] private BrickData _dataStandard;
    [SerializeField] private BrickData _dataReinforced;
    [SerializeField] private BrickData _dataIndestructible;
    [SerializeField] private BallController _ball;
    [SerializeField] private PowerUpManager _powerUpManager;

    [SerializeField] private float _cellWidth  = 1.8f;
    [SerializeField] private float _cellHeight = 0.6f;
    [SerializeField] private float _gridTop    = 5.0f;

    private List<GameObject> _bricks = new();
    private int _remainingBricks;

    void Awake() => Instance = this;

    void Start()
    {
        if (GameManager.Instance == null) { Debug.LogWarning("BrickManager: GameManager.Instance is null."); return; }
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        LoadLevel(GameManager.Instance.CurrentLevelIndex);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        BrickDestroyed = null;
        BrickDamaged   = null;
        LevelCleared   = null;
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // A brick reached 0 HP. The manager fires events — audio, effects, and game state
    // are handled by subscribers, not here.
    public void OnBrickDestroyed(BrickData data, Vector3 position)
    {
        BrickDestroyed?.Invoke(position, data.FullHealthColor);

        ScoreManager.Instance?.AddScore(data.PointValue);
        _powerUpManager?.TrySpawnDrop(position);

        _remainingBricks--;
        if (_remainingBricks <= 0)
            LevelCleared?.Invoke();
    }

    public void OnBrickDamaged()
    {
        BrickDamaged?.Invoke();
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.LevelComplete && GameManager.Instance != null)
            LoadLevel(GameManager.Instance.CurrentLevelIndex);
    }

    private void LoadLevel(int index)
    {
        ClearGrid();
        if (_levels == null || index >= _levels.Length)
        {
            Debug.LogWarning($"BrickManager: No LevelData for index {index}.");
            return;
        }
        var data = _levels[index];
        SpawnGrid(data);
        if (_ball != null)
            _ball.SetLevelMultiplier(data.BallSpeedMultiplier);
    }

    private void ClearGrid()
    {
        foreach (var b in _bricks) if (b != null) Destroy(b);
        _bricks.Clear();
        _remainingBricks = 0;
    }

    private void SpawnGrid(LevelData data)
    {
        float startX = -(data.Columns - 1) * _cellWidth * 0.5f;
        for (int r = 0; r < data.Rows; r++)
        for (int c = 0; c < data.Columns; c++)
        {
            int i = r * data.Columns + c;
            if (i >= data.Grid.Length) continue;
            var type = data.Grid[i];
            if (type == BrickType.None) continue;

            var pos   = new Vector3(startX + c * _cellWidth, _gridTop - r * _cellHeight, 0f);
            var go    = Instantiate(_brickPrefab, pos, Quaternion.identity, transform);
            var brick = go.GetComponent<Brick>();
            bool destructible = type != BrickType.Indestructible;
            brick.Init(GetData(type), this, destructible);
            if (destructible) _remainingBricks++;
            _bricks.Add(go);
        }
    }

    private BrickData GetData(BrickType type) => type switch
    {
        BrickType.Standard       => _dataStandard,
        BrickType.Reinforced     => _dataReinforced,
        BrickType.Indestructible => _dataIndestructible,
        _                        => _dataStandard,
    };
}
