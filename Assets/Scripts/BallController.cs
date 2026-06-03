using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    [SerializeField] private PaddleController _paddle;
    [SerializeField] public float BaseSpeed = 8f;
    [SerializeField] private float _minSpeed = 6f;
    [SerializeField] private float _maxSpeed = 18f;
    [SerializeField] public bool IsMain = true;

    public static BallController Instance { get; private set; }
    public float Speed    => _rb.linearVelocity.magnitude;
    public Vector2 Velocity => _rb.linearVelocity;

    // Waiting: on paddle, idle. Flying: in play. Bouncing: mid-collision. Dead: respawning.
    private enum BallState { Waiting, Flying, Bouncing, Dead }
    private BallState _state = BallState.Waiting;

    private const float MAX_BOUNCE_ANGLE = 60f;
    private const float RESPAWN_DELAY    = 1.5f;
    private const float PADDLE_OFFSET_Y  = 0.4f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private float _currentSpeed;
    private float _levelMultiplier = 1f;
    private readonly List<float> _speedModifiers = new();

    private float CombinedModifier
    {
        get { float r = 1f; foreach (var m in _speedModifiers) r *= m; return r; }
    }

    private float EffectiveSpeed => Mathf.Clamp(BaseSpeed * _levelMultiplier * CombinedModifier, _minSpeed, _maxSpeed);

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        if (IsMain)
        {
            Instance = this;
            if (_paddle == null) Debug.LogWarning("BallController: PaddleController not assigned.");
        }
    }

    void Start()
    {
        if (IsMain)
        {
            _currentSpeed = EffectiveSpeed;
            GoToWaiting();
        }
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        if (IsMain && Instance == this) Instance = null;
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (!IsMain) return;
        if (state == GameManager.GameState.LevelComplete)
            GoToWaiting();
    }

    void Update()
    {
        if (_state == BallState.Waiting && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            Launch();
    }

    void FixedUpdate()
    {
        if (_state == BallState.Waiting) SnapToPaddle();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (_state == BallState.Flying) _state = BallState.Bouncing;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (_state == BallState.Bouncing) _state = BallState.Flying;
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
        if (!IsMain) { Destroy(gameObject); return; }
        _state = BallState.Dead;
        CameraEffects.Instance?.Shake(0.25f, 0.35f);
        AudioManager.Instance?.Play(AudioManager.Instance.SfxBallLost);
        if (GameManager.Instance != null) GameManager.Instance.OnBallLost();
        StartCoroutine(RespawnAfterDelay());
    }

    // Per-level base scaling, owned by BrickManager.
    public void SetLevelMultiplier(float multiplier)
    {
        _levelMultiplier = multiplier;
        ApplySpeed();
    }

    public void AddSpeedModifier(float modifier)  { _speedModifiers.Add(modifier);    ApplySpeed(); }
    public void RemoveSpeedModifier(float modifier){ _speedModifiers.Remove(modifier); ApplySpeed(); }
    public void ClearSpeedModifiers()             { _speedModifiers.Clear();           ApplySpeed(); }

    public void InitAsAuxiliary(Vector2 velocity, Color color)
    {
        IsMain = false;
        _state = BallState.Flying;
        _currentSpeed = velocity.magnitude;
        if (_sr != null) _sr.color = color;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.linearVelocity = velocity;
    }

    private void ApplySpeed()
    {
        _currentSpeed = EffectiveSpeed;
        if ((_state == BallState.Flying || _state == BallState.Bouncing) &&
            _rb.bodyType == RigidbodyType2D.Dynamic && _rb.linearVelocity != Vector2.zero)
            _rb.linearVelocity = _rb.linearVelocity.normalized * _currentSpeed;
    }

    private void GoToWaiting()
    {
        _state = BallState.Waiting;
        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        SnapToPaddle();
    }

    private void Launch()
    {
        _state = BallState.Flying;
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
        GoToWaiting();
    }
}
