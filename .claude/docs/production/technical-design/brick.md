# Brick

> **Status**: Approved
> **Last Updated**: 2026-06-01
> **Implements Pillar**: Explosive — every brick hit is a visible, satisfying destruction event

## Summary

Brick is a MonoBehaviour on each Brick prefab instance. It tracks hit points from its `BrickData`, updates its `SpriteRenderer` color on damage, and reports each hit/death up to BrickManager. It is a dumb entity: it owns no effects, scoring, or drops — BrickManager is the coordinator for all of those. Its only outward dependency is the `BrickManager` reference passed to `Init()`.

> **Quick reference** — Layer: `Core Loop` · Priority: `MVP` · Key deps: `BrickData`, `BrickManager`

---

## Overview

Brick has no Update loop — it is purely reactive. All logic fires from `OnCollisionEnter2D`. Brick is initialized by BrickManager at spawn time via `Init()` rather than using serialized Inspector fields (since it's instantiated at runtime from a prefab).

## Player Fantasy

Every brick that gets hit responds visually — Reinforced bricks darken, Standard bricks pop. The player gets clear feedback that they're making progress.

---

## Detailed Design

### Core Rules

1. Brick is initialized via `Init(BrickData, BrickManager, bool destructible)` — called by BrickManager immediately after `Instantiate`.
2. `HitPoints` and `PointValue` come from `BrickData` — never hardcoded.
3. `OnCollisionEnter2D` calls `TakeDamage(1)`. If `_destructible == false`, the call is skipped.
4. On damage, `SpriteRenderer.color` lerps from `DamagedColor` to `FullHealthColor` based on remaining HP fraction.
5. When `_hp <= 0`, Brick calls `_manager.OnBrickDestroyed(_data, transform.position)`, then destroys itself.
6. When a hit is survived (`_hp > 0`), Brick calls `_manager.OnBrickDamaged()` so the manager plays hit feedback.
7. Brick references no other system — no CameraEffects, ParticlePool, AudioManager, ScoreManager, or PowerUpManager. Score, drops, and all juice are BrickManager's responsibility.
8. Brick never calls `FindObjectOfType` — its only reference (`BrickManager`) comes from `Init()`.

### States and Transitions

No state machine — Brick is stateless between collisions.

### Interactions with Other Systems

| System | Interaction |
|---|---|
| `BrickManager` | Calls `OnBrickDestroyed(data, position)` on death and `OnBrickDamaged()` on a survived hit; reference passed via `Init()` |
| `BallController` | Collision resolved by Unity physics; `OnCollisionEnter2D` fires on Brick |

---

## Formulas

### Color Lerp on Damage
```
t = currentHP / maxHP       // 0 = dead, 1 = full health
color = Lerp(DamagedColor, FullHealthColor, t)
```

Only applied when `maxHP > 1` (i.e., Reinforced bricks). Standard bricks do not change color.

---

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|---|---|---|
| Ball hits Indestructible brick | `TakeDamage` skipped — no HP change | `_destructible == false` guard |
| Ball hits Reinforced brick at HP=1 | Color lerp → t=0 = DamagedColor, then dies next frame | Correct; lerp handles it |
| `Init()` not called before collision | `_data` is null; NullRef in TakeDamage | BrickManager always calls Init immediately after Instantiate |
| Multiple ball contacts in one frame | Only one `OnCollisionEnter2D` fires per collision event | Unity physics guarantee |

---

## Dependencies

| System | Direction | Nature |
|---|---|---|
| `BrickData` | This depends on it | Data dependency — HP, color, point value |
| `BrickManager` | This depends on it | State trigger — notifies on death |

---

## Tuning Knobs

All tuning is in `BrickData` ScriptableObject assets — none in Brick.cs.

| Asset field | Default | Effect |
|---|---|---|
| `BrickData_Standard.HitPoints` | 1 | One-shot destruction |
| `BrickData_Reinforced.HitPoints` | 2 | Two hits to destroy |
| `BrickData_Standard.PointValue` | 100 | Points awarded on death |
| `BrickData_Reinforced.PointValue` | 250 | Higher reward for harder bricks |

---

## Visual / Audio Requirements

> Brick itself only drives the damage-color lerp on its own SpriteRenderer. Every other effect below (SFX, particles, hitstop, shake) is produced by **BrickManager** in `OnBrickDamaged()` / `OnBrickDestroyed()` — Brick just reports the event.

| Event | Visual | Audio | Owner | Priority |
|---|---|---|---|---|
| Hit (Reinforced, HP > 1) | Color darkens toward DamagedColor | Soft crack SFX | Brick (color) + BrickManager (SFX/hitstop) | MVP |
| Death (any destructible) | GameObject destroyed | Pop/crack SFX | BrickManager | MVP |
| Death (VFX) | Particle burst | — | BrickManager | Polish |

---

## Acceptance Criteria

- [ ] Standard brick destroyed in 1 hit
- [ ] Reinforced brick changes color on first hit, destroyed on second
- [ ] Indestructible brick takes no damage
- [ ] `BrickManager.OnBrickDestroyed(data, position)` called exactly once per destroyed brick
- [ ] No hardcoded HP values — all from BrickData
- [ ] No `FindObjectOfType` in implementation

---

## Open Questions

None.
