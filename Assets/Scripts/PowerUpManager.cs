using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject _powerUpPrefab;
    [SerializeField] private GameObject _ballPrefab;

    [SerializeField] private float _dropChance    = 0.55f;
    [SerializeField] private float _fallSpeed     = 5f;
    [SerializeField] private float _speedBoostMul = 1.5f;
    [SerializeField] private float _speedBoostDur = 5f;

    private readonly BasePowerUp[] _powerUps = { new TripleBallPowerUp(), new SpeedBoostPowerUp() };
    private readonly List<BallController> _auxBalls = new();
    private Coroutine _speedBoostCoroutine;
    private float _activeSpeedBoostMul;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            GameManager.Instance.OnLivesChanged     += OnLivesChanged;
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            GameManager.Instance.OnLivesChanged     -= OnLivesChanged;
        }
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.LevelComplete)
        {
            ClearAuxBalls();
            StopSpeedBoost();
        }
    }

    private void OnLivesChanged(int _)
    {
        ClearAuxBalls();
        StopSpeedBoost();
    }

    public void TrySpawnDrop(Vector3 position)
    {
        if (_powerUpPrefab == null) return;
        if (Random.value > _dropChance) return;
        var effect = _powerUps[Random.Range(0, _powerUps.Length)];
        var go = Instantiate(_powerUpPrefab, position, Quaternion.identity);
        go.GetComponent<PowerUp>().Init(effect, _fallSpeed, this);
    }

    public void ApplyTripleBall() => SpawnTripleBalls();

    public void ApplySpeedBoost()
    {
        if (_speedBoostCoroutine != null) StopCoroutine(_speedBoostCoroutine);
        _speedBoostCoroutine = StartCoroutine(SpeedBoostEffect());
    }

    private void SpawnTripleBalls()
    {
        if (_ballPrefab == null || BallController.Instance == null) return;
        Vector2 baseVel = BallController.Instance.Velocity;
        if (baseVel == Vector2.zero) return;
        float speed = baseVel.magnitude;
        var magenta = new Color(1f, 0.15f, 0.85f);
        SpawnAuxBall(Rotate(baseVel,  30f).normalized * speed, magenta);
        SpawnAuxBall(Rotate(baseVel, -30f).normalized * speed, magenta);
    }

    private void SpawnAuxBall(Vector2 velocity, Color color)
    {
        var go   = Instantiate(_ballPrefab, BallController.Instance.transform.position, Quaternion.identity);
        var ball = go.GetComponent<BallController>();
        ball.InitAsAuxiliary(velocity, color);
        _auxBalls.Add(ball);
    }

    private IEnumerator SpeedBoostEffect()
    {
        _activeSpeedBoostMul = _speedBoostMul;
        BallController.Instance?.AddSpeedModifier(_activeSpeedBoostMul);
        yield return new WaitForSeconds(_speedBoostDur);
        BallController.Instance?.RemoveSpeedModifier(_activeSpeedBoostMul);
        _speedBoostCoroutine = null;
    }

    private void StopSpeedBoost()
    {
        if (_speedBoostCoroutine == null) return;
        StopCoroutine(_speedBoostCoroutine);
        _speedBoostCoroutine = null;
        BallController.Instance?.RemoveSpeedModifier(_activeSpeedBoostMul);
    }

    private void ClearAuxBalls()
    {
        foreach (var b in _auxBalls)
            if (b != null) Destroy(b.gameObject);
        _auxBalls.Clear();
    }

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
