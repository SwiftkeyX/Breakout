# PowerUpManager

> **Status**: Approved
> **Last Updated**: 2026-05-31
> **Implements Pillar**: Chaotic — random drops that explode the rules mid-game

## Summary

PowerUpManager lives in the Game scene, manages the lifecycle of falling PowerUp drops, and applies timed effects to PaddleController or BallController on pickup. Brick holds a reference passed at spawn time. Two types at Tier 3: ExpandPaddle and SlowBall. MultiBall deferred to Phase 3.

> **Quick reference** — Layer: `Supporting` · Priority: `MVP` · Key deps: `PaddleController`, `BallController`

---

## Detailed Design

### Core Rules

1. PowerUpManager is NOT a singleton. BrickManager holds a serialized reference and passes it to `Brick.Init()`.
2. `TrySpawnDrop(Vector3)` is called by Brick on death. It rolls `Random.value` against `_dropChance` (default 0.3). If pass: pick random `PowerUpType`, instantiate PowerUp prefab, call `PowerUp.Init()`.
3. `OnPickup(PowerUpType)` is called by PowerUp on Paddle collision. Starts the appropriate coroutine.
4. Each effect type has its own coroutine that applies the effect, waits `N` seconds, then reverts.
5. If a PowerUp of the same type is picked up while already active, the timer resets (coroutine restarts).
6. PowerUp objects that exit the play area bottom are destroyed by `OnBecameInvisible()`.

### PowerUp Types

| Type | Effect | Duration | Revert |
|---|---|---|---|
| ExpandPaddle | `PaddleController.SetWidth(5f)` | 10s | `SetWidth(original)` |
| SlowBall | `BallController.SetSpeed(base × 0.6)` | 8s | `SetSpeed(original)` |

### Visual Coding

| Type | Color |
|---|---|
| ExpandPaddle | Green `(0.4, 1.0, 0.4)` |
| SlowBall | Yellow `(1.0, 0.8, 0.2)` |

---

## Tuning Knobs

| Parameter | Default | Effect |
|---|---|---|
| `_dropChance` | 0.3 | Probability per brick death |
| `_fallSpeed` | 3f | World units/sec downward |
| `_expandScaleX` | 5f | Paddle width during effect |
| `_expandDuration` | 10s | ExpandPaddle active time |
| `_slowMultiplier` | 0.6 | BallSpeed fraction during SlowBall |
| `_slowDuration` | 8s | SlowBall active time |

---

## Acceptance Criteria

- [ ] ~30% of destroyed bricks drop a PowerUp
- [ ] Green drop widens paddle for 10s then reverts
- [ ] Yellow drop slows ball for 8s then reverts
- [ ] PowerUps that miss the paddle disappear off-screen
- [ ] No FindObjectOfType in implementation

---

## Open Questions
- MultiBall (deferred to Phase 3 juice pass)
