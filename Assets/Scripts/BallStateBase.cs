using UnityEngine;

// Add ChangeState(), use this to transition between state
// Each children need to implement it 
public abstract class BallStateBase
{
    protected BallController Ball { get; }

    protected BallStateBase(BallController ball) { Ball = ball; }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnCollisionEnter2D(Collision2D col) { }
    public virtual void OnCollisionExit2D(Collision2D col) { }
    public virtual void OnTriggerEnter2D(Collider2D other) { }
}
