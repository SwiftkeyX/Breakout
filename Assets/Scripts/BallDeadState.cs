using System.Collections;
using UnityEngine;

public class BallDeadState : BallStateBase
{
    private const float RESPAWN_DELAY = 1.5f;

    public BallDeadState(BallController ball) : base(ball) { }

    public override void Enter()
    {
        Ball.Rb.linearVelocity = Vector2.zero;
        Ball.Rb.bodyType = RigidbodyType2D.Kinematic;
        CameraEffects.Instance?.Shake(0.25f, 0.35f);
        AudioManager.Instance?.Play(AudioManager.Instance.SfxBallLost);
        if (GameManager.Instance != null) GameManager.Instance.OnBallLost();
        Ball.StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(RESPAWN_DELAY);
        Ball.TransitionTo(Ball.WaitingState);
    }
}
