using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;

    public int Score { get; private set; }

    void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    public void AddScore(int amount)
    {
        float speed = BallController.Instance != null ? BallController.Instance.Speed : 0f;
        float multiplier = speed > 0f ? Mathf.Max(1f, speed / 10f) : 1f;
        Score += Mathf.RoundToInt(amount * multiplier);
        OnScoreChanged?.Invoke(Score);
    }

    public void Reset()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }
}
