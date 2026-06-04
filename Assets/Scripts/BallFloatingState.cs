using UnityEngine;

public class BallFloatingState : BallStateBase
{
    public BallFloatingState(BallController ball) : base(ball) { }

    public override void Enter()
    {
        Ball.Rb.bodyType = RigidbodyType2D.Dynamic;
        if (Ball.IsMain && Ball.Rb.linearVelocity == Vector2.zero)
            Ball.Rb.linearVelocity = Vector2.up * Ball.CurrentSpeed;
    }

    public override void OnCollisionEnter2D(Collision2D col)
    {
        Ball.TransitionTo(Ball.HittingState);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("DeathZone")) return;
        if (!Ball.IsMain) { Object.Destroy(Ball.gameObject); return; }
        Ball.TransitionTo(Ball.DeadState);
    }
}
