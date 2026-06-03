using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject _powerUpPrefab;
    [SerializeField] private BallController _ball;

    [SerializeField] private float _dropChance      = 0.55f;
    [SerializeField] private float _fallSpeed       = 5f;
    [SerializeField] private float _boostDuration   = 5f;
    [SerializeField] private float _boostMultiplier = 2.0f;
    [SerializeField] private Color _auxBallColor    = new Color(1f, 0.2f, 0.7f);

    private Coroutine _boostCoroutine;
    private readonly List<BallController> _auxBalls = new();

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            GameManager.Instance.OnLivesChanged += OnLivesChanged;
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            GameManager.Instance.OnLivesChanged -= OnLivesChanged;
        }
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.LevelComplete) return;
        if (_boostCoroutine != null) { StopCoroutine(_boostCoroutine); _boostCoroutine = null; }
        if (_ball != null) _ball.ClearSpeedModifiers();
        ClearAuxBalls();
    }

    private void OnLivesChanged(int _) => ClearAuxBalls();

    private void ClearAuxBalls()
    {
        foreach (var b in _auxBalls) if (b != null) Destroy(b.gameObject);
        _auxBalls.Clear();
    }

    public void TrySpawnDrop(Vector3 position)
    {
        if (_powerUpPrefab == null) return;
        if (Random.value > _dropChance) return;
        var type = (PowerUpType)Random.Range(0, 2);
        var go = Instantiate(_powerUpPrefab, position, Quaternion.identity);
        go.GetComponent<PowerUp>().Init(type, _fallSpeed, this);
    }

    public void OnPickup(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.TripleBall:
                SpawnAuxBalls();
                break;
            case PowerUpType.SpeedBoost:
                if (_boostCoroutine != null) StopCoroutine(_boostCoroutine);
                _boostCoroutine = StartCoroutine(SpeedBoostEffect());
                break;
        }
    }

    private void SpawnAuxBalls()
    {
        if (_ball == null) return;
        Vector2 vel = _ball.Velocity;
        if (vel == Vector2.zero) vel = Vector2.up * _ball.BaseSpeed;
        SpawnAuxBall(Rotate(vel, 30f));
        SpawnAuxBall(Rotate(vel, -30f));
    }

    private void SpawnAuxBall(Vector2 velocity)
    {
        var go  = Instantiate(_ball.gameObject, _ball.transform.position, _ball.transform.rotation);
        var aux = go.GetComponent<BallController>();
        aux.InitAsAuxiliary(velocity, _auxBallColor);
        _auxBalls.Add(aux);
    }

    private IEnumerator SpeedBoostEffect()
    {
        if (_ball == null) yield break;
        _ball.AddSpeedModifier(_boostMultiplier);
        yield return new WaitForSeconds(_boostDuration);
        _ball.RemoveSpeedModifier(_boostMultiplier);
        _boostCoroutine = null;
    }

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
