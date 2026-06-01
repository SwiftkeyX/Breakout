using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject _powerUpPrefab;
    [SerializeField] private PaddleController _paddle;
    [SerializeField] private BallController _ball;

    [SerializeField] private float _dropChance     = 0.3f;
    [SerializeField] private float _fallSpeed      = 3f;
    [SerializeField] private float _expandDuration = 10f;
    [SerializeField] private float _expandScaleX   = 5f;
    [SerializeField] private float _slowDuration   = 8f;
    [SerializeField] private float _slowMultiplier = 0.6f;

    private Coroutine _expandCoroutine;
    private Coroutine _slowCoroutine;
    private float _paddleBaseScaleX = 1f;

    void Start()
    {
        if (_paddle != null) _paddleBaseScaleX = _paddle.transform.localScale.x;
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.LevelComplete) return;
        if (_expandCoroutine != null) { StopCoroutine(_expandCoroutine); _expandCoroutine = null; }
        if (_slowCoroutine   != null) { StopCoroutine(_slowCoroutine);   _slowCoroutine   = null; }
        if (_paddle != null) _paddle.SetWidth(_paddleBaseScaleX);
        if (_ball   != null) _ball.SetSpeedModifier(1f);
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
            case PowerUpType.ExpandPaddle:
                if (_expandCoroutine != null) StopCoroutine(_expandCoroutine);
                _expandCoroutine = StartCoroutine(ExpandPaddleEffect());
                break;
            case PowerUpType.SlowBall:
                if (_slowCoroutine != null) StopCoroutine(_slowCoroutine);
                _slowCoroutine = StartCoroutine(SlowBallEffect());
                break;
        }
    }

    private IEnumerator ExpandPaddleEffect()
    {
        if (_paddle == null) yield break;
        _paddle.SetWidth(_expandScaleX);
        yield return new WaitForSeconds(_expandDuration);
        _paddle.SetWidth(_paddleBaseScaleX);
        _expandCoroutine = null;
    }

    private IEnumerator SlowBallEffect()
    {
        if (_ball == null) yield break;
        _ball.SetSpeedModifier(_slowMultiplier);
        yield return new WaitForSeconds(_slowDuration);
        _ball.SetSpeedModifier(1f);
        _slowCoroutine = null;
    }
}
