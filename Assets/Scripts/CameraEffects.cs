using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance { get; private set; }

    private Vector3 _origin;
    private Coroutine _shakeCoroutine;

    void Awake()
    {
        Instance = this;
        _origin = transform.position;
    }

    void Start()
    {
        if (BrickManager.Instance == null) return;
        BrickManager.Instance.BrickDestroyed += OnBrickDestroyed;
        BrickManager.Instance.BrickDamaged   += OnBrickDamaged;
        BrickManager.Instance.LevelCleared   += OnLevelCleared;
    }

    void OnDestroy()
    {
        if (BrickManager.Instance != null)
        {
            BrickManager.Instance.BrickDestroyed -= OnBrickDestroyed;
            BrickManager.Instance.BrickDamaged   -= OnBrickDamaged;
            BrickManager.Instance.LevelCleared   -= OnLevelCleared;
        }
    }

    private void OnBrickDestroyed(Vector3 _, Color __) { HitStop(0.06f); Shake(0.08f, 0.15f); }
    private void OnBrickDamaged()                      => HitStop(0.03f);
    private void OnLevelCleared()                      => Shake(0.20f, 0.40f);

    public void HitStop(float seconds) => StartCoroutine(HitStopRoutine(seconds));

    public void Shake(float magnitude, float duration)
    {
        if (_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
        _shakeCoroutine = StartCoroutine(ShakeRoutine(magnitude, duration));
    }

    private IEnumerator HitStopRoutine(float seconds)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1f;
    }

    private IEnumerator ShakeRoutine(float magnitude, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float falloff = 1f - Mathf.Clamp01(elapsed / duration);
            Vector2 offset = Random.insideUnitCircle * magnitude * falloff;
            transform.position = _origin + new Vector3(offset.x, offset.y, 0f);
            yield return null;
        }
        transform.position = _origin;
        _shakeCoroutine = null;
    }
}
