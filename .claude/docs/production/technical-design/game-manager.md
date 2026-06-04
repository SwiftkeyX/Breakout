# GameManager

> **Status**: Approved
> **Last Updated**: 2026-06-01
> **Implements Pillar**: Fun — short, replayable sessions require clean state ownership and fast transitions

## Summary

GameManager is the singleton that owns all game-wide state. It holds the `GameState` enum, lives count, and current level index. Every state transition in the game flows through here.

> **Quick reference** — Layer: `Foundation` · Priority: `MVP` · Key deps: `None`

---

## Overview

GameManager is the only singleton in the project. It persists across all scenes via `DontDestroyOnLoad`. Other systems call its public methods to signal events (ball lost, level cleared); GameManager decides what happens next (decrement lives, advance level, load scene).

## Player Fantasy

The player should never feel stuck — if they lose a life or clear a level, the game responds immediately and decisively. No hesitation.

---

## Detailed Design

### Core Rules

1. Only one instance ever exists. If a second is created, it destroys itself immediately.
2. `DontDestroyOnLoad` is called in `Awake` — GameManager lives for the full application lifetime.
3. All scene transitions are delegated to `SceneLoader` — GameManager never calls `SceneManager` directly.
4. State changes always fire `OnGameStateChanged` — no silent transitions.
5. Lives start at `STARTING_LIVES` (3) when `StartGame()` is called. They are never modified outside GameManager.
6. `CurrentLevelIndex` is zero-based. Level 1 is index 0.
7. `TotalLevels` is a public read-only property exposing `TOTAL_LEVELS` — consumers (e.g. UIManager's win check) must read it instead of hardcoding the level count.
8. `LevelComplete` is a transient signal, not a resting state: `OnLevelComplete()` fires it (BrickManager reloads the grid synchronously on that event) and then immediately re-enters `Playing`, so per-ball logic gated on `Playing` keeps working in the next level.

### States and Transitions

| State | Entry Condition | Exit Condition | Behavior |
|---|---|---|---|
| `MainMenu` | App launch or `ReturnToMainMenu()` | `StartGame()` called | Idle; no game logic runs |
| `Playing` | `StartGame()` or next level loaded | Ball lost or all bricks cleared | Active gameplay |
| `LevelComplete` | All bricks cleared, more levels remain | Re-enters `Playing` in the same call | Transient signal — `BrickManager` reloads the layout synchronously, then state returns to `Playing` |
| `GameOver` | All lives lost or all levels cleared | `RestartGame()` or `ReturnToMainMenu()` | GameOver scene loaded |

### Interactions with Other Systems

| System | Interaction |
|---|---|
| `SceneLoader` | GameManager calls `SceneLoader.Load()` for all transitions |
| `BrickManager` | Subscribes to `OnGameStateChanged` (reloads grid on `LevelComplete`); calls `OnLevelComplete()` when the grid is cleared |
| `ScoreManager` | Calls `GameManager.Instance.OnGameOver()` when lives hit 0 (via `OnBallLost` flow) |
| `UIManager` | Subscribes to `OnGameStateChanged` to show/hide screens |

---

## Formulas

None — lives and level index are integer counters with no formula.

---

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|---|---|---|
| Second GameManager spawned | Destroy the duplicate immediately in `Awake` | Prevents doubled event subscriptions |
| `OnBallLost` called when lives = 1 | Decrement to 0, trigger GameOver | Zero lives = game over, not negative |
| `OnLevelComplete` at final level (index 4) | Load GameOver scene | All levels complete = win; reuse GameOver scene for now |
| `StartGame()` called mid-game | Reset lives and level index fully | Restart is a full reset, not a resume |

---

## Dependencies

| System | Direction | Nature |
|---|---|---|
| `SceneLoader` | This depends on it | State trigger — loads scenes on state change |

---

## Tuning Knobs

| Parameter | Default | Safe Range | Effect of Increase | Effect of Decrease |
|---|---|---|---|---|
| `STARTING_LIVES` | 3 | 1–5 | More forgiving | Punishing one-life runs |
| `TOTAL_LEVELS` | 5 | 1–10 | Longer game | Shorter vertical slice |

---

## Visual / Audio Requirements

None at Tier 1. UIManager (Tier 3) handles all state-driven UI.

---

## Acceptance Criteria

- [ ] `GameManager.Instance` is non-null after MainMenu loads
- [ ] Calling `StartGame()` sets `State == Playing`, `Lives == 3`, `CurrentLevelIndex == 0`
- [ ] Calling `OnBallLost()` three times from `Playing` state triggers GameOver scene load
- [ ] `OnGameStateChanged` fires on every state transition
- [ ] No `FindObjectOfType` or `GameObject.Find` in implementation
- [ ] All tuning knobs are named constants, not magic numbers
- [ ] Performance: `SetState` completes within 1ms

---

## Open Questions

None.
