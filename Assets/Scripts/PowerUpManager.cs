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
        float original = _paddle.transform.localScale.x;
        _paddle.SetWidth(_expandScaleX);
        yield return new WaitForSeconds(_expandDuration);
        _paddle.SetWidth(original);
        _expandCoroutine = null;
    }

    private IEnumerator SlowBallEffect()
    {
        if (_ball == null) yield break;
        float original = _ball.BaseSpeed;
        _ball.SetSpeed(original * _slowMultiplier);
        yield return new WaitForSeconds(_slowDuration);
        _ball.SetSpeed(original);
        _slowCoroutine = null;
    }
}
