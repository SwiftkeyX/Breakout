# PowerUpManager

> **Status**: Approved
> **Last Updated**: 2026-06-04
> **Implements Pillar**: Chaotic — random drops that explode the rules mid-game

## Summary

PowerUpManager lives in the Game scene, manages the lifecycle of falling PowerUp drops, and applies timed effects to PaddleController or BallController on pickup. BrickManager holds a serialized reference and calls `TrySpawnDrop()` on brick death. Two types at Tier 3: SpeedBoost and TripleBall.

> **Quick reference** — Layer: `Supporting` · Priority: `MVP` · Key deps: `BallController`, `PaddleController`

---

## Detailed Design

### Architecture

Power-up types are implemented as a class hierarchy. `BasePowerUp` is an abstract base class with a `Color` property and a `PickUp(PowerUpManager)` method. `SpeedBoostPowerUp` and `TripleBallPowerUp` are concrete subclasses serialized into `PowerUpManager._powerUps[]`. The falling `PowerUp.cs` prefab controller calls `effect.PickUp(manager)` on paddle collision — it has no knowledge of the effect type.

### Core Rules

1. PowerUpManager is NOT a singleton. BrickManager holds a serialized reference and calls `TrySpawnDrop()` directly on brick death.
2. `TrySpawnDrop(Vector3)` rolls `Random.value` against `_dropChance`. If pass: pick a random `BasePowerUp` from `_powerUps[]`, instantiate the PowerUp prefab, call `PowerUp.Init(effect, fallSpeed, manager)`.
3. `PowerUp.cs` detects Paddle collision and calls `effect.PickUp(manager)`, then destroys itself.
4. Each `BasePowerUp` subclass implements `PickUp(PowerUpManager)` as a coroutine that applies its effect, waits its duration, then reverts.
5. If the same effect type is picked up while already active, the coroutine restarts (timer resets).
6. PowerUp objects that exit the play area bottom are destroyed by `OnBecameInvisible()`.
7. SpeedBoost applies via `BallController.SetSpeedModifier()`, a composable factor — the per-level multiplier survives the effect and is restored correctly on expiry.
8. TripleBall auxiliary balls are tracked by PowerUpManager and destroyed on level complete or when lives change, preventing carry-over between lives.

### PowerUp Types

| Type | Effect | Duration | Revert |
|---|---|---|---|
| SpeedBoost | `BallController.SetSpeedModifier(1.5)` | 5s | `SetSpeedModifier(1f)` |
| TripleBall | Spawns 2 auxiliary balls at ±30° from main ball | Until life lost or level cleared | Auxiliary balls destroyed |

### Visual Coding

| Type | Color |
|---|---|
| SpeedBoost | Orange `(1.0, 0.5, 0.0)` |
| TripleBall | Magenta `(1.0, 0.15, 0.85)` |

---

## Tuning Knobs

| Parameter | Default | Effect |
|---|---|---|
| `_dropChance` | 0.55 | Probability per brick death |
| `_fallSpeed` | 5f | World units/sec downward |
| `_speedBoostMul` | 1.5 | Ball speed multiplier during SpeedBoost |
| `_speedBoostDur` | 5s | SpeedBoost active time |

---

## Acceptance Criteria

- [ ] ~55% of destroyed bricks drop a PowerUp
- [ ] Orange drop increases ball speed by 1.5× for 5s then reverts
- [ ] Magenta drop spawns 2 auxiliary balls at ±30° angles
- [ ] Auxiliary balls are destroyed on life lost or level cleared
- [ ] PowerUps that miss the paddle disappear off-screen
- [ ] No FindObjectOfType in implementation

---

## Open Questions

- Should TripleBall timer reset if picked up again while active?
- Future: MultiBall expansion (more than 2 auxiliary balls)?
