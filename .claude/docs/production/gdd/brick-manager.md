# BrickManager

> **Status**: Approved
> **Last Updated**: 2026-06-01
> **Implements Pillar**: Fun — clean level transitions keep the run feeling snappy and continuous

## Summary

BrickManager reads a `LevelData` ScriptableObject and instantiates a Brick grid at the start of each level. It tracks the remaining destructible Brick count and calls `GameManager.Instance.OnLevelComplete()` when it hits zero. It subscribes to `GameManager.OnGameStateChanged` to reload the grid when the next level starts.

> **Quick reference** — Layer: `Core Loop` · Priority: `MVP` · Key deps: `GameManager`, `LevelData`, `BrickData`

---

## Overview

BrickManager is a MonoBehaviour (not a singleton — GameManager is the only permitted singleton). Brick objects hold a reference to BrickManager passed at spawn time. Bricks stay dumb: on a surviving hit they call `OnBrickDamaged()`, and on death they call `OnBrickDestroyed(data, position)`. BrickManager is the coordinator that owns every resulting effect — camera hitstop/shake, particle burst, SFX, score award, and the PowerUp drop roll — so that fan-out lives in one place instead of in every Brick instance.

## Player Fantasy

The transition from one level to the next is instant and satisfying — the grid refreshes, the ball resets, and the player is immediately ready for the next challenge.

---

## Detailed Design

### Core Rules

1. BrickManager subscribes to `GameManager.OnGameStateChanged` in `Start()` and unsubscribes in `OnDestroy()`.
2. On `Start()`, BrickManager immediately calls `LoadLevel(GameManager.Instance.CurrentLevelIndex)` to handle the initial load (the Playing event has already fired before this scene loaded).
3. When `GameManager.State == LevelComplete`, BrickManager calls `LoadLevel(GameManager.Instance.CurrentLevelIndex)` — which now points to the *new* index (GameManager already incremented it).
4. `LoadLevel` destroys all existing Brick instances, resets `_remainingBricks` to 0, then spawns the new grid.
5. Only `BrickType.Standard` and `BrickType.Reinforced` bricks count toward `_remainingBricks`. `Indestructible` bricks never decrement the counter.
6. When `_remainingBricks` reaches 0 (from `OnBrickDestroyed` calls), BrickManager calls `GameManager.Instance.OnLevelComplete()`.
7. All tunable values (cell size, grid top Y) are serialized Inspector fields.
8. BrickManager is **not** a singleton — Brick holds a direct reference assigned at spawn.
9. `OnBrickDestroyed(BrickData data, Vector3 position)` owns the full death sequence in order: camera hitstop + shake, particle burst, break SFX, `ScoreManager.AddScore(data.PointValue)`, `PowerUpManager.TrySpawnDrop(position)`, then the count decrement / level-complete check.
10. `OnBrickDamaged()` owns the surviving-hit feedback: brief hitstop + hit SFX. Bricks never touch CameraEffects/ParticlePool/AudioManager/ScoreManager/PowerUpManager directly.

### States and Transitions

No explicit states — BrickManager reacts to GameManager state changes.

### Interactions with Other Systems

| System | Interaction |
|---|---|
| `GameManager` | Subscribes to `OnGameStateChanged`; calls `OnLevelComplete()` when grid cleared |
| `Brick` | Receives `OnBrickDestroyed(data, position)` / `OnBrickDamaged()`; passes `this` reference at spawn via `Init()` |
| `BallController` | Calls `SetLevelMultiplier(multiplier)` when loading each level |
| `CameraEffects` | Calls `HitStop()` / `Shake()` on brick hit and death |
| `ParticlePool` | Calls `Burst(position, color)` on brick death |
| `AudioManager` | Plays hit / break / level-clear SFX |
| `ScoreManager` | Calls `AddScore(data.PointValue)` on brick death |
| `PowerUpManager` | Calls `TrySpawnDrop(position)` on brick death |
| `LevelData` | Reads `Rows`, `Columns`, `Grid[]`, `BallSpeedMultiplier` |
| `BrickData` | Passes correct data to each Brick via `Init()` |

---

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|---|---|---|
| `_levels` array shorter than level index | Log warning, return early — no grid | Prevents index out of range |
| All bricks are Indestructible | `_remainingBricks = 0` at spawn; `OnLevelComplete` fires immediately | Handled naturally — don't create all-indestructible levels |
| `OnBrickDestroyed` called after level already cleared | `_remainingBricks` goes negative | Acceptable; `OnLevelComplete` already fired; guard not needed |
| GameManager.Instance null in Start | Log warning, do not subscribe or load | Defensive guard |

---

## Dependencies

| System | Direction | Nature |
|---|---|---|
| `GameManager` | This depends on it | State trigger — reacts to state changes, calls OnLevelComplete |
| `BallController` | This depends on it | State trigger — sets level speed multiplier |
| `LevelData` | This depends on it | Data dependency — reads grid layout |
| `BrickData` | This depends on it | Data dependency — passes to Brick.Init |
| `CameraEffects` / `ParticlePool` / `AudioManager` | This depends on them | Effect trigger — brick hit/death juice (optional singletons, null-guarded) |
| `ScoreManager` | This depends on it | State trigger — awards points on brick death |
| `PowerUpManager` | This depends on it | State trigger — rolls drops on brick death (serialized reference) |

---

## Tuning Knobs

| Parameter | Default | Safe Range | Effect of Increase | Effect of Decrease |
|---|---|---|---|---|
| `_cellWidth` | 1.8 | 1.2–2.5 | Wider bricks, more spacing | Narrower bricks, tighter grid |
| `_cellHeight` | 0.6 | 0.4–1.0 | Taller bricks | Shorter bricks |
| `_gridTop` | 5.0 | 3.0–5.5 | Grid starts higher | Grid starts lower |

---

## Acceptance Criteria

- [ ] Brick grid spawns correctly for each of the 5 LevelData assets
- [ ] Destroying all destructible bricks calls `GameManager.Instance.OnLevelComplete()`
- [ ] Indestructible bricks are never counted in `_remainingBricks`
- [ ] `OnDestroy` unsubscribes from `OnGameStateChanged` — no event leak
- [ ] Level 2 grid loads after Level 1 is cleared without scene reload
- [ ] Brick death produces particle burst, SFX, score, and drop roll — all driven by BrickManager, not Brick
- [ ] No `FindObjectOfType` or `GameObject.Find` in implementation

---

## Open Questions

None.
