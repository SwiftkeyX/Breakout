using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;

    public float Width => _collider.bounds.size.x;

    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private float _leftBound;
    private float _rightBound;

    private const float PADDLE_Y = -4.5f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        if (_inputHandler == null) Debug.LogWarning("PaddleController: InputHandler not assigned.");
    }

    void Start()
    {
        var cam = Camera.main;
        if (cam == null) { Debug.LogWarning("PaddleController: Camera.main is null."); return; }
        float halfW = cam.orthographicSize * cam.aspect;
        float halfPaddleW = _collider.bounds.extents.x;
        _leftBound  = -halfW + halfPaddleW;
        _rightBound =  halfW - halfPaddleW;
        transform.position = new Vector3(0f, PADDLE_Y, 0f);
    }

    void FixedUpdate()
    {
        if (_inputHandler == null) return;
        float targetX = Mathf.Clamp(_inputHandler.PaddleTargetX, _leftBound, _rightBound);
        _rb.MovePosition(new Vector2(targetX, PADDLE_Y));
    }
}
