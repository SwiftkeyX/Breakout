using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private PaddleController _paddle;
    [SerializeField] public float BaseSpeed = 8f;
    [SerializeField] public float MinSpeed  = 6f;
    [SerializeField] public float MaxSpeed  = 18f;
    [SerializeField] public bool IsMain = true;

    public static BallController Instance { get; private set; }

    public Rigidbody2D    Rb  { get; private set; }
    public SpriteRenderer Sr  { get; private set; }
    public PaddleController Paddle => _paddle;
    public float Speed    => Rb.linearVelocity.magnitude;
    public Vector2 Velocity => Rb.linearVelocity;
    public float CurrentSpeed { get; private set; }

    public BallStateBase WaitingState  { get; private set; }
    public BallStateBase FloatingState { get; private set; }
    public BallStateBase HittingState  { get; private set; }
    public BallStateBase DeadState     { get; private set; }

    private const float PADDLE_OFFSET_Y = 0.4f;

    private BallStateBase _state;
    private float _levelMultiplier = 1f;
    private readonly List<float> _speedModifiers = new();

    private float CombinedModifier
    {
        get { float r = 1f; foreach (var m in _speedModifiers) r *= m; return r; }
    }

    public float EffectiveSpeed => Mathf.Clamp(BaseSpeed * _levelMultiplier * CombinedModifier, MinSpeed, MaxSpeed);

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponent<SpriteRenderer>();
        WaitingState  = new BallWaitingState(this);
        FloatingState = new BallFloatingState(this);
        HittingState  = new BallHittingState(this);
        DeadState     = new BallDeadState(this);
    }

    void Start()
    {
        if (IsMain)
        {
            Instance = this;
            if (_paddle == null) Debug.LogWarning("BallController: PaddleController not assigned.");
            CurrentSpeed = EffectiveSpeed;
            TransitionTo(WaitingState);
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
            TransitionTo(WaitingState);
    }

    public void TransitionTo(BallStateBase newState)
    {
        _state?.Exit();
        _state = newState;
        _state.Enter();
    }

    void Update()       => _state?.Update();
    void FixedUpdate()  => _state?.FixedUpdate();

    void OnCollisionEnter2D(Collision2D col) => _state?.OnCollisionEnter2D(col);
    void OnCollisionExit2D(Collision2D col)  => _state?.OnCollisionExit2D(col);
    void OnTriggerEnter2D(Collider2D other)  => _state?.OnTriggerEnter2D(other);

    public void SetCurrentSpeed(float speed) { CurrentSpeed = speed; }

    public void SetLevelMultiplier(float multiplier)
    {
        _levelMultiplier = multiplier;
        ApplySpeed();
    }

    public void AddSpeedModifier(float m)    { _speedModifiers.Add(m);    ApplySpeed(); }
    public void RemoveSpeedModifier(float m) { _speedModifiers.Remove(m); ApplySpeed(); }
    public void ClearSpeedModifiers()        { _speedModifiers.Clear();   ApplySpeed(); }

    public void ApplySpeed()
    {
        CurrentSpeed = EffectiveSpeed;
        if (Rb == null) return;
        if ((_state is BallFloatingState || _state is BallHittingState) &&
            Rb.bodyType == RigidbodyType2D.Dynamic && Rb.linearVelocity != Vector2.zero)
            Rb.linearVelocity = Rb.linearVelocity.normalized * CurrentSpeed;
    }

    public void SnapToPaddlePosition()
    {
        if (_paddle == null) return;
        var pos = _paddle.transform.position;
        transform.position = new Vector3(pos.x, pos.y + PADDLE_OFFSET_Y, 0f);
    }

    public void InitAsAuxiliary(Vector2 velocity, Color color)
    {
        IsMain = false;
        if (Sr != null) Sr.color = color;
        CurrentSpeed = velocity.magnitude;
        TransitionTo(FloatingState);
        Rb.linearVelocity = velocity;
    }
}
