using UnityEngine;

public abstract class BallStateBase
{
    protected BallController Ball { get; }

    protected BallStateBase(BallController ball) { Ball = ball; }

    protected void ChangeState(BallStateBase next) => Ball.TransitionTo(next);

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnCollisionEnter2D(Collision2D col) { }
    public virtual void OnCollisionExit2D(Collision2D col) { }
    public virtual void OnTriggerEnter2D(Collider2D other) { }
}
