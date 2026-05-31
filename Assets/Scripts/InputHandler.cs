using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public float PaddleTargetX { get; private set; }

    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
        if (_camera == null) Debug.LogWarning("InputHandler: Camera.main is null.");
    }

    void Update()
    {
        if (_camera == null) return;
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _camera.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, _camera.nearClipPlane));
        PaddleTargetX = worldPos.x;
    }
}
