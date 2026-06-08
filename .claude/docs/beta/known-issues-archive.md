# Known Issues — Archive

> Fixed and Won't Fix bugs. Read for historical context only — do not log new bugs here. New bugs go to `known-issues.md`.

## Fixed

| # | Description | Fixed in |
|---|---|---|
| 1 | Ball falls below boundary without life loss; keeps falling indefinitely | Fixed 2026-06-02 — DeathZone BoxCollider2D height increased from 0.2 → 2.0 units |
| 2 | Ball freezes just below the play area on the final life; game over screen never loads and game is stuck | Fixed 2026-06-02 — Removed GameState check from `RespawnAfterDelay`; `GoToWaiting()` now called unconditionally (coroutine stops naturally when ball is destroyed on scene unload) |
| 3 | Level transition does not fire after all bricks cleared when Play Mode started via coplay `play_game` | Fixed 2026-06-08 — Moved `LevelCleared` subscription from `GameManager.OnSceneLoaded` into a new `RegisterBrickManager()` method called from `BrickManager.Start()` |

### #3 — [BrickManager] Level transition does not fire after all bricks cleared in Level 1
- **Environment:** Unity 6.4, Windows 11, Game scene (Level 1), Play Mode
- **Description:** Player is left in an empty level with no way to progress, making the game feel broken/unfinished.
- **Steps to Reproduce:**
  1. Let Claude start Play Mode via the coplay MCP `play_game` tool (not by entering Play Mode manually)
  2. Play Level 1 and destroy all bricks
  3. Observe: no level transition occurs (could not reproduce when entering Play Mode manually)
- **Expected:** After the last brick is destroyed in Level 1, the game automatically loads Level 2.
- **Actual:** All bricks cleared; paddle still controllable, ball keeps bouncing, UI frozen at Level 1 / 3 lives / score 0 — no transition occurs.
- **Evidence:** none observed
- **Area:** BrickManager / SceneLoader / GameManager
- **Severity:** Major
- **Fixed in:** 2026-06-08 — Moved `LevelCleared` subscription from `GameManager.OnSceneLoaded` (which fired before `BrickManager.Awake` in coplay) into a new `RegisterBrickManager()` method called from `BrickManager.Start()`, guaranteeing subscription after all Awake calls complete

## Won't Fix

| # | Description | Reason |
|---|---|---|
| — | *(none yet)* | |
