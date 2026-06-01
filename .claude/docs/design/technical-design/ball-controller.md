# BallController

> **Status**: Approved
> **Last Updated**: 2026-06-01
> **Implements Pillar**: Chaotic · Explosive — unpredictable bounces + angle control = satisfying, replayable chaos

## Summary

BallController manages the Ball's two states (Waiting on paddle / Moving in play), launches on left-click, computes bounce angles from paddle hit position, clamps minimum speed after every bounce, and handles death + respawn. It is the most physics-critical script in the project.

> **Quick reference** — Layer: `Core Loop` · Priority: `MVP` · Key deps: `PaddleController`, `GameManager`

---

## Overview

The Ball uses Unity's 2D physics (`Rigidbody2D` Dynamic) for bouncing, but BallController overrides the velocity direction after every collision. This gives precise, predictable behaviour while keeping the physics engine handling penetration resolution.

## Player Fantasy

The ball feels snappy and energetic. Hitting the edge of the paddle sends it racing at a sharp angle; hitting the centre sends it straight up. The player develops intuition for angles and uses them to aim at specific bricks.

---

## Detailed Design

### Core Rules

1. Ball has two states: `Waiting` (kinematic, following paddle) and `Moving` (dynamic, in flight).
2. In `Waiting`, the ball follows `PaddleController.transform.position` each `FixedUpdate`.
3. Launch triggers on `Mouse.current.leftButton.wasPressedThisFrame` — ball enters `Moving` state, fires straight up at `_currentSpeed`.
4. `OnCollisionExit2D` normalises velocity to `_currentSpeed` after every bounce — prevents energy drift.
5. Paddle bounces override direction using the hit-position angle formula (see Formulas).
6. Speed is always clamped above `_minSpeed` — see best-practices.md "Ball Never Stops" rule.
7. When the Ball enters the `DeathZone` trigger (bottom of screen), it calls `GameManager.Instance.OnBallLost()` then starts a respawn coroutine.
8. After `RESPAWN_DELAY` seconds, if `GameManager.State == Playing`, ball returns to `Waiting` state on the paddle.
9. BallController is the single owner of effective speed: `EffectiveSpeed = BaseSpeed × _levelMultiplier × _speedModifier`, clamped to `_minSpeed`. `BaseSpeed` is a serialized Inspector field.
10. The two speed inputs compose and never clobber each other: BrickManager sets the per-level scaling via `SetLevelMultiplier()`, and PowerUpManager sets transient effects (SlowBall) via `SetSpeedModifier()`. Each setter recomputes `_currentSpeed` and re-applies it to the current velocity direction.
11. Launch input reads `Mouse.current` directly and is null-guarded — no mouse device means no launch (no exception).

### States and Transitions

| State | Entry Condition | Exit Condition | Behavior |
|---|---|---|---|
| `Waiting` | Scene load, or after respawn delay | Left mouse click | Kinematic; snaps to paddle X; waits for click |
| `Moving` | Left mouse click | Ball enters DeathZone trigger | Dynamic; velocity-driven; bounces off walls/bricks/paddle |

### Interactions with Other Systems

| System | Interaction |
|---|---|
| `PaddleController` | Reads `transform.position` and `bounds.extents.x` for respawn snap and bounce angle |
| `GameManager` | Calls `GameManager.Instance.OnBallLost()` on DeathZone entry |
| `BrickManager` | Calls `SetLevelMultiplier(multiplier)` when loading a new level |
| `PowerUpManager` | Calls `SetSpeedModifier(factor)` for SlowBall, and `SetSpeedModifier(1f)` on expiry |
| Physics engine | `OnCollisionExit2D` fires after every bounce for velocity override |

---

## Formulas

### Paddle Bounce Angle
```
normalizedHit = clamp((ball.x - paddle.x) / paddle.halfWidth, -1, 1)
angle         = normalizedHit * MAX_BOUNCE_ANGLE   // in degrees
direction     = Vector2(sin(angle_rad), cos(angle_rad))
velocity      = direction * _currentSpeed
```

| Variable | Range | Effect |
|---|---|---|
| `normalizedHit` | −1 to +1 | −1 = sharp left, 0 = straight up, +1 = sharp right |
| `MAX_BOUNCE_ANGLE` | 60° (default) | Max deflection from vertical |

**Expected**: ball always travels upward after paddle contact (Y component always positive).

### Minimum Speed Clamp
```
if (velocity.magnitude < _minSpeed)
    velocity = velocity.normalized * _minSpeed
```

---

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|---|---|---|
| Ball hits paddle corner | Physics resolves collision; OnCollisionExit2D normalises velocity | No special case needed |
| Ball moving nearly horizontally | Minimum speed clamp maintains momentum | Prevents infinite horizontal loop |
| GameManager.Instance is null on DeathZone | Log warning, skip OnBallLost call | Defensive null check |
| Respawn coroutine fires but state = GameOver | Skip GoToWaiting; do nothing | Scene already transitioning |
| Multiple DeathZone triggers in same frame | Only the first `OnTriggerEnter2D` fires; coroutine already started | Not an issue in practice |
| No mouse device present (`Mouse.current == null`) | Launch check is skipped; ball stays in `Waiting` | Null-guarded — no NullReferenceException |
| SlowBall active when a new level loads | Level multiplier and slow modifier compose; both apply | Setters are independent factors, not overwrites |

---

## Dependencies

| System | Direction | Nature |
|---|---|---|
| `PaddleController` | This depends on it | Data dependency — position for snap + bounds for angle |
| `GameManager` | This depends on it | State trigger — notifies on ball lost |
| `PhysicsMaterial2D` | This depends on it | Rule dependency — bounciness=1 required for correct bounce |

---

## Tuning Knobs

| Parameter | Default | Safe Range | Effect of Increase | Effect of Decrease |
|---|---|---|---|---|
| `_baseSpeed` | 8.0 | 5–15 | Harder, more chaotic | Easier, more controllable |
| `_minSpeed` | 5.0 | 3–8 | Ball never slows below this | Lower = allow slower ball |
| `MAX_BOUNCE_ANGLE` | 60° | 30°–75° | Wilder angles, more chaos | Straighter bounces, less control |
| `RESPAWN_DELAY` | 1.5s | 0.5–3.0s | More breathing room | Faster punishment |

---

## Visual / Audio Requirements

| Event | Visual | Audio | Priority |
|---|---|---|---|
| Ball launch | — | — | Polish |
| Ball hits brick | — | Soft click SFX | MVP |
| Ball hits paddle | — | Lower click SFX | MVP |
| Ball enters DeathZone | Ball disappears | Low thud SFX | MVP |
| Ball respawn | Ball fades in on paddle | — | Polish |

---

## Game Feel

### Feel Reference
> "Should feel like Arkanoid's ball physics — tight, snappy, player-controllable angle from paddle. NOT floaty or random."

### Input Responsiveness

| Action | Max Latency | Frame Budget |
|---|---|---|
| Click → ball launches | 16.6ms (1 frame) | 1 frame |

### Weight and Responsiveness
- **Weight**: Light and fast — high speed, instant direction change on paddle
- **Player control**: Angle fully determined by paddle hit position — fully readable
- **Snap quality**: Binary launch (waiting → full speed instantly)
- **Failure texture**: When ball is missed, player can always see the angle that beat them

---

## Acceptance Criteria

- [ ] Ball launches straight up on first left-click
- [ ] Ball angle from paddle varies smoothly from −60° to +60° based on hit position
- [ ] Speed never drops below `_minSpeed` after any bounce
- [ ] Ball respawns on paddle after 1.5s when life > 0
- [ ] No respawn if GameManager.State != Playing
- [ ] No `FindObjectOfType` in implementation
- [ ] `_baseSpeed` and `_minSpeed` are Inspector-exposed, not hardcoded

---

## Open Questions

None.
