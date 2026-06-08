# InputHandler

> **Status**: Approved
> **Last Updated**: 2026-06-01
> **Implements Pillar**: Chaotic — precise mouse tracking lets the player stay in control even when the screen is chaos

## Summary

InputHandler reads raw mouse position each frame via the Input System package and converts it to a world-space X coordinate. It exposes `PaddleTargetX` as a public property. PaddleController reads this to move the Paddle.

> **Quick reference** — Layer: `Foundation` · Priority: `MVP` · Key deps: `Camera.main`

---

## Overview

Paddle-movement input is read through InputHandler: PaddleController reads `InputHandler.PaddleTargetX` each frame and does not own input logic. The one exception is ball launch — BallController reads `Mouse.current.leftButton` directly for the click-to-launch action.

## Player Fantasy

The Paddle tracks the mouse with zero perceived lag — the player feels directly connected to the action.

---

## Detailed Design

### Core Rules

1. InputHandler is a MonoBehaviour attached to a persistent GameObject in the Game scene.
2. Mouse position is read from `Mouse.current.position.ReadValue()` — never from the legacy `Input` class.
3. Screen space is converted to world space using `Camera.main.ScreenToWorldPoint`.
4. Only the X component is exposed — Y position of the Paddle is fixed and not controlled by input.
5. If `Camera.main` is null (logged in `Awake`) **or** `Mouse.current` is null, InputHandler skips the `Update` calculation — no null reference exception.
6. PaddleController holds a direct Inspector reference to InputHandler — no `FindObjectOfType`.

### States and Transitions

No states — runs every frame in `Update` while the Game scene is loaded.

### Interactions with Other Systems

| System | Interaction |
|---|---|
| `PaddleController` | Reads `PaddleTargetX` directly each frame (Inspector reference) |
| `Camera.main` | Used for screen→world conversion; cached in `Awake` |

---

## Formulas

### Screen to World X

```
worldPos = Camera.ScreenToWorldPoint(Vector3(mouseX, mouseY, camera.nearClipPlane))
PaddleTargetX = worldPos.x
```

| Variable | Type | Range | Source | Description |
|---|---|---|---|---|
| `mouseX` | float | 0 – screen width (px) | `Mouse.current.position` | Raw screen X |
| `mouseY` | float | 0 – screen height (px) | `Mouse.current.position` | Raw screen Y (unused after conversion) |
| `nearClipPlane` | float | engine default (~0.3) | `Camera.main` | Depth for 2D orthographic projection |
| `PaddleTargetX` | float | play area left–right | calculated | World X passed to PaddleController |

**Expected output range**: camera left edge to camera right edge (approximately −8.9 to +8.9 at orthographic size 6, 16:9)

---

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|---|---|---|
| `Camera.main` is null at Awake | Log warning, skip Update | Prevents NullReferenceException spam |
| `Mouse.current` is null (no mouse device) | Skip Update calculation that frame | Null-guarded — no NullReferenceException |
| Mouse outside screen bounds | Unity clamps mouse to window; value follows | No special handling needed |
| Game paused (`Time.timeScale = 0`) | `Update` still runs; input still tracked | Paddle should remain responsive during pause |

---

## Dependencies

| System | Direction | Nature |
|---|---|---|
| `Camera.main` | This depends on it | Data dependency — used for coordinate conversion |
| `PaddleController` | It depends on this | Data dependency — reads `PaddleTargetX` each frame |

---

## Tuning Knobs

None — mouse position mapping is direct with no multiplier or smoothing at this tier.

---

## Visual / Audio Requirements

None — InputHandler has no visual representation.

---

## Game Feel

### Feel Reference

> "Should feel like classic Breakout's paddle tracking — 1:1 mouse-to-paddle mapping with no smoothing or acceleration. NOT sluggish or dampened."

### Input Responsiveness

| Action | Max Input-to-Response Latency | Frame Budget (60fps) |
|---|---|---|
| Mouse move → Paddle position update | 16.6ms (1 frame) | 1 frame |

### Weight and Responsiveness

- **Weight**: Light and reactive — paddle snaps to mouse position
- **Player control**: Fully course-correctable at any moment
- **Snap quality**: Crisp and binary — no lerp or smoothing
- **Failure texture**: If the ball is missed, the player can always see why (paddle position was accurate)

---

## Acceptance Criteria

- [ ] `PaddleTargetX` updates every frame to match mouse world X
- [ ] No `Input.GetAxis` or legacy input API used
- [ ] `Camera.main` null path logs a warning and does not throw
- [ ] PaddleController wired via Inspector reference, not `FindObjectOfType`
- [ ] Performance: `Update` completes in under 0.1ms

---

## Open Questions

None.
