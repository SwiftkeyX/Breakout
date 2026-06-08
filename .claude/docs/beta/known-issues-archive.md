# Known Issues — Archive

> Fixed and Won't Fix bugs. Read for historical context only — do not log new bugs here. New bugs go to `known-issues.md`.

## Fixed

| # | Description | Fixed in |
|---|---|---|
| 1 | Ball falls below boundary without life loss; keeps falling indefinitely | Fixed 2026-06-02 — DeathZone BoxCollider2D height increased from 0.2 → 2.0 units |
| 2 | Ball freezes just below the play area on the final life; game over screen never loads and game is stuck | Fixed 2026-06-02 — Removed GameState check from `RespawnAfterDelay`; `GoToWaiting()` now called unconditionally (coroutine stops naturally when ball is destroyed on scene unload) |
| 3 | Level transition does not fire after all bricks cleared when Play Mode started via coplay `play_game` | Fixed 2026-06-08 — Moved `LevelCleared` subscription from `GameManager.OnSceneLoaded` into a new `RegisterBrickManager()` method called from `BrickManager.Start()` |
| 4 | [PowerUp] Triple ball pickup plays sound but spawns no extra balls | Fixed 2026-06-08 — Assigned `Assets/Prefabs/Ball.prefab` to `_ballPrefab` on PowerUpManager in Game scene |
| 5 | [Build] Triple ball power-up has no effect in standalone build — `_ballPrefab` null at runtime | Fixed 2026-06-08 — `set_property` only writes to editor runtime memory; `save_scene` was saving to `Assets/Game.unity` (duplicate) instead of `Assets/Scenes/Game.unity`. Fixed via `SerializedObject` editor script writing directly to correct scene path. |

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

### #4 — [PowerUp] Triple ball pickup plays sound but spawns no extra balls
- **Environment:** Unity 6.4, Windows 11, Game scene, Play Mode
- **Description:** The player cannot experience multi-ball gameplay — triple ball is a major feature and collecting the drop has no effect beyond the pickup sound.
- **Steps to Reproduce:**
  1. Open Game scene, enter Play Mode
  2. Bounce the ball to destroy bricks until a triple-ball power-up drop falls
  3. Move the paddle to collect the drop
- **Expected:** Three balls bounce simultaneously — the original ball splits into two additional balls at ±30° angles.
- **Actual:** The pickup sound played, the drop disappeared on contact, but no additional balls appeared.
- **Evidence:** None observed
- **Area:** PowerUpManager / TripleBallPowerUp
- **Severity:** Major
- **Fixed in:** 2026-06-08 — `_ballPrefab` was not assigned in the Inspector on the PowerUpManager component in Game scene; assigned `Assets/Prefabs/Ball.prefab` via `set_property`

### #5 — [Build] Triple ball power-up has no effect in standalone build
- **Environment:** Unity 6.4.9f1, Windows 11, standalone Windows build
- **Description:** The player collects the triple ball drop and hears the pickup sound but no extra balls spawn — only the original ball continues.
- **Steps to Reproduce:**
  1. Build the project for Windows (`File → Build Settings → Build`)
  2. Launch the built `.exe`
  3. Play until a triple-ball power-up drop falls and collect it
- **Expected:** Two extra magenta balls spawn at ±30° angles.
- **Actual:** Pickup sound plays, drop disappears, no extra balls appear.
- **Evidence:** None observed (no console in standalone build)
- **Area:** PowerUpManager / Assets/Scenes/Game.unity serialization
- **Severity:** Major
- **Fixed in:** 2026-06-08 — Root cause: `set_property` (coplay MCP) only writes to editor runtime memory, not the scene file. `save_scene` with `scene_name: "Game"` was creating a duplicate at `Assets/Game.unity` instead of saving `Assets/Scenes/Game.unity`. Fixed by writing a `FixPowerUpManager.cs` editor script using `SerializedObject` to set `_ballPrefab` directly in `Assets/Scenes/Game.unity`.

## Won't Fix

| # | Description | Reason |
|---|---|---|
| — | *(none yet)* | |
