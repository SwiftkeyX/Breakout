using UnityEngine;
using UnityEngine.InputSystem;

public class BallWaitingState : BallStateBase
{
    public BallWaitingState(BallController ball) : base(ball) { }

    public override void Enter()
    {
        Ball.Rb.linearVelocity = Vector2.zero;
        Ball.Rb.bodyType = RigidbodyType2D.Kinematic;
        Ball.SnapToPaddlePosition();
    }

    public override void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            ChangeState(Ball.FloatingState);
    }

    public override void FixedUpdate() => Ball.SnapToPaddlePosition();
}
