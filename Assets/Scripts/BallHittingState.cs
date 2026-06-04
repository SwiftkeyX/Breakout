using UnityEngine;

public class BallHittingState : BallStateBase
{
    private const float MAX_BOUNCE_ANGLE = 60f;

    public BallHittingState(BallController ball) : base(ball) { }

    public override void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Paddle"))
        {
            float norm = Mathf.Clamp(
                (Ball.transform.position.x - col.transform.position.x) / col.collider.bounds.extents.x,
                -1f, 1f);
            float angle = norm * MAX_BOUNCE_ANGLE * Mathf.Deg2Rad;
            Ball.Rb.linearVelocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * Ball.CurrentSpeed;
            AudioManager.Instance?.Play(AudioManager.Instance.SfxHitPaddle);
        }
        else
        {
            Ball.Rb.linearVelocity = Ball.Rb.linearVelocity.normalized * Ball.CurrentSpeed;
            ClampMinSpeed();
            if (col.gameObject.CompareTag("Wall"))
                AudioManager.Instance?.Play(AudioManager.Instance.SfxHitWall);
        }

        Ball.TransitionTo(Ball.FloatingState);
    }

    // Guard: handles the unlikely case the ball reaches DeathZone while mid-bounce.
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("DeathZone")) return;
        if (!Ball.IsMain) { Object.Destroy(Ball.gameObject); return; }
        Ball.TransitionTo(Ball.DeadState);
    }

    private void ClampMinSpeed()
    {
        if (Ball.Rb.linearVelocity.magnitude < Ball.MinSpeed)
            Ball.Rb.linearVelocity = Ball.Rb.linearVelocity.normalized * Ball.MinSpeed;
    }
}
