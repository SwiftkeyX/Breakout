# PaddleController

> **Status**: Approved
> **Last Updated**: 2026-05-31
> **Implements Pillar**: Chaotic — precise tracking keeps the player in control as the screen fills with chaos

## Summary

PaddleController moves the Paddle horizontally in response to `InputHandler.PaddleTargetX`, clamps it to the play area edges, and exposes a `Width` property for PowerUp size modifications. It uses a kinematic Rigidbody2D so the ball interacts with it cleanly via Unity physics.

> **Quick reference** — Layer: `Core Loop` · Priority: `MVP` · Key deps: `InputHandler`

---

## Overview

The Paddle is the player's only direct control over the game. It sits at a fixed Y position near the bottom of the play area and tracks mouse X position 1:1. No smoothing, no acceleration — direct mapping for maximum responsiveness.

## Player Fantasy

The Paddle feels like an extension of the player's hand. Moving the mouse left and right snaps the Paddle exactly there. The player never blames the controls for missing a ball.

---

## Detailed Design

### Core Rules

1. Paddle moves along the X axis only — Y position is fixed at `PADDLE_Y` (−4.5 world units).
2. X position is read from `InputHandler.PaddleTargetX` every `FixedUpdate` via `Rigidbody2D.MovePosition`.
3. X is clamped to `[leftBound, rightBound]` computed from `Camera.main` bounds minus half the Paddle width.
4. `Rigidbody2D` type is Kinematic with Interpolate enabled — required for smooth ball collision response.
5. `Width` property reflects the current `BoxCollider2D.bounds.size.x` — it changes if a PowerUp resizes the SpriteRenderer/Collider.
6. `InputHandler` reference is set in the Inspector — never looked up via FindObjectOfType.

### States and Transitions

No explicit state machine — PaddleController runs continuously while in the Game scene.

### Interactions with Other Systems

| System | Interaction |
|---|---|
| `InputHandler` | Reads `PaddleTargetX` each FixedUpdate |
| `BallController` | Ball reads `transform.position` + `Width` for respawn positioning and bounce angle |
| `PowerUpManager` | Calls `SetWidth(float)` to temporarily resize the Paddle (Tier 3) |

---

## Formulas

### Camera Bounds
```
halfWidth = camera.orthographicSize * camera.aspect
leftBound  = -halfWidth + paddleHalfWidth
rightBound =  halfWidth - paddleHalfWidth
```

---

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|---|---|---|
| InputHandler reference is null | Log warning in Awake, skip FixedUpdate | No crash; Paddle stays stationary |
| Camera.main is null in Start | Log warning, use fallback bounds (±9) | Prevents NullRef; still functional |
| Mouse at screen edge | X clamped to `[leftBound, rightBound]` | Paddle never exits play area |

---

## Dependencies

| System | Direction | Nature |
|---|---|---|
| `InputHandler` | This depends on it | Data dependency — reads PaddleTargetX |
| `BallController` | It depends on this | Data dependency — reads position and Width |
| `PowerUpManager` | It depends on this | Rule dependency — resizes via SetWidth (Tier 3) |

---

## Tuning Knobs

| Parameter | Default | Safe Range | Effect of Increase | Effect of Decrease |
|---|---|---|---|---|
| `PADDLE_Y` | −4.5 | −5 to −3.5 | Paddle closer to bottom | Paddle closer to brick zone |
| Paddle width (SpriteRenderer scale X) | 3.0 | 1.5–5.0 | Easier | Harder |
| Paddle height (scale Y) | 0.3 | 0.2–0.5 | More forgiving angle window | Thinner, less forgiving |

---

## Acceptance Criteria

- [ ] Paddle tracks mouse X with no visible lag (1-frame response)
- [ ] Paddle never exits the left or right screen edge
- [ ] Paddle Y stays fixed at `PADDLE_Y` regardless of mouse Y
- [ ] `Width` property returns correct value after resize
- [ ] No `FindObjectOfType` in implementation
- [ ] No magic numbers — all constants named

---

## Open Questions

None.
