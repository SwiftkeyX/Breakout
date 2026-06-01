# ScoreManager

> **Status**: Approved
> **Last Updated**: 2026-06-01
> **Implements Pillar**: Fun — visible score feedback makes every brick feel rewarding

## Summary

ScoreManager is a component singleton attached to the GameManager's DontDestroyOnLoad GameObject. It tracks the current score, fires `OnScoreChanged` events for UIManager, and resets on new game. It does NOT track lives — those remain in GameManager.

> **Quick reference** — Layer: `Supporting` · Priority: `MVP` · Key deps: `GameManager (host)`

---

## Overview

Because GameManager persists across scenes via `DontDestroyOnLoad`, attaching ScoreManager to the same GameObject gives it the same lifetime without adding a second true singleton. `ScoreManager.Instance` is set in `Awake` and is safe to use from any scene.

## Player Fantasy

Every brick the player hits flashes a score increase in the HUD. Over the course of a level, the growing score tells a story of progress.

---

## Detailed Design

### Core Rules

1. ScoreManager is a MonoBehaviour component on the GameManager's GameObject — never instantiated separately.
2. `Instance` is set in `Awake`. It is always valid as long as GameManager exists.
3. `AddScore(int)` increments Score and fires `OnScoreChanged`.
4. `Reset()` sets Score to 0 and fires `OnScoreChanged`. Called by GameManager.StartGame().
5. ScoreManager has no knowledge of lives — that belongs to GameManager.
6. Point values come from `BrickData.PointValue` — ScoreManager never hardcodes them.

### Interactions with Other Systems

| System | Interaction |
|---|---|
| `GameManager` | Hosts ScoreManager; calls `Reset()` on `StartGame()` |
| `BrickManager` | Calls `ScoreManager.Instance.AddScore(data.PointValue)` on brick death |
| `UIManager` | Subscribes to `OnScoreChanged` to update HUD label |

---

## Edge Cases

| Scenario | Expected Behavior |
|---|---|
| `AddScore` called before `Reset` on new game | `StartGame` calls `Reset` before scene loads — safe |
| Score rolls past int.MaxValue | Not possible in a 5-level game; ignore |

---

## Acceptance Criteria

- [ ] Score increments by correct point value for Standard (100) and Reinforced (250) bricks
- [ ] HUD updates immediately on each score change
- [ ] Score resets to 0 at the start of each new game
- [ ] `Instance` is non-null in Game and GameOver scenes

---

## Open Questions
None.
