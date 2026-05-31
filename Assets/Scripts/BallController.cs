using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    [SerializeField] private PaddleController _paddle;
    [SerializeField] public float BaseSpeed = 8f;
    [SerializeField] private float _minSpeed = 5f;

    public float Speed => _rb.linearVelocity.magnitude;

    private const float MAX_BOUNCE_ANGLE = 60f;
    private const float RESPAWN_DELAY    = 1.5f;
    private const float PADDLE_OFFSET_Y  = 0.4f;

    private Rigidbody2D _rb;
    private bool _waiting = true;
    private float _currentSpeed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_paddle == null) Debug.LogWarning("BallController: PaddleController not assigned.");
    }

    void Start()
    {
        _currentSpeed = BaseSpeed;
        GoToWaiting();
    }

    void Update()
    {
        if (_waiting && Mouse.current.leftButton.wasPressedThisFrame)
            Launch();
    }

    void FixedUpdate()
    {
        if (_waiting) SnapToPaddle();
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Paddle"))
        {
            float norm = Mathf.Clamp(
                (transform.position.x - col.transform.position.x) / col.collider.bounds.extents.x,
                -1f, 1f);
            float angle = norm * MAX_BOUNCE_ANGLE * Mathf.Deg2Rad;
            _rb.linearVelocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * _currentSpeed;
            AudioManager.Instance?.Play(AudioManager.Instance.SfxHitPaddle);
            return;
        }
        _rb.linearVelocity = _rb.linearVelocity.normalized * _currentSpeed;
        ClampMinSpeed();
        if (col.gameObject.CompareTag("Wall"))
            AudioManager.Instance?.Play(AudioManager.Instance.SfxHitWall);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("DeathZone")) return;
        CameraEffects.Instance?.Shake(0.25f, 0.35f);
        AudioManager.Instance?.Play(AudioManager.Instance.SfxBallLost);
        if (GameManager.Instance != null) GameManager.Instance.OnBallLost();
        StartCoroutine(RespawnAfterDelay());
    }

    public void SetSpeed(float speed)
    {
        _currentSpeed = Mathf.Max(speed, _minSpeed);
    }

    private void GoToWaiting()
    {
        _waiting = true;
        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        SnapToPaddle();
    }

    private void Launch()
    {
        _waiting = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.linearVelocity = Vector2.up * _currentSpeed;
    }

    private void SnapToPaddle()
    {
        if (_paddle == null) return;
        var pos = _paddle.transform.position;
        transform.position = new Vector3(pos.x, pos.y + PADDLE_OFFSET_Y, 0f);
    }

    private void ClampMinSpeed()
    {
        if (_rb.linearVelocity.magnitude < _minSpeed)
            _rb.linearVelocity = _rb.linearVelocity.normalized * _minSpeed;
    }

    private IEnumerator RespawnAfterDelay()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(RESPAWN_DELAY);
        if (GameManager.Instance != null &&
            GameManager.Instance.State == GameManager.GameState.Playing)
            GoToWaiting();
    }
}
