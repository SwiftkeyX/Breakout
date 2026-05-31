# Architecture Contract

> The authoritative list of every script in this project, its single responsibility, and how scripts communicate. Fill this in as systems are implemented. Claude reads this before touching any existing script.
>
> For coding conventions and anti-patterns, see `.claude/template-docs/technical/coding-style.md`.
> For which systems are planned and their tier, see `.claude/docs/design/systems-design.md`.

## Script Table

| Script | Responsibility |
|---|---|
| **GameManager** | Singleton; owns `GameState` enum; holds lives count and current level index; entry point for game-wide state changes |
| **SceneLoader** | Wraps `SceneManager.LoadSceneAsync`; called by GameManager to transition between MainMenu, Game, and GameOver |
| **InputHandler** | Reads `Mouse.current.position` each frame via Input System; exposes `PaddleTargetX` (world space) to PaddleController |
| **PaddleController** | Moves Paddle transform to `InputHandler.PaddleTargetX`; clamps to play area bounds; exposes `Width` property for PowerUp size changes |
| **BallController** | Moves Ball via `Rigidbody2D`; normalises and scales velocity after each bounce; clamps speed above `minSpeed`; exposes `Speed` for PowerUp modifications |
| **BrickManager** | Instantiates Brick grid from level data ScriptableObject; tracks `remainingBricks` counter; fires `OnAllBricksCleared` event when counter hits zero |
| **Brick** | Component on each Brick prefab; tracks `hitPoints`; on taking damage decrements HP, updates visual, and on death instantiates destruction VFX and calls `PowerUpManager.TrySpawnDrop` |
| **PowerUpManager** | Singleton; owns pool of falling PowerUp objects; moves them downward; on Paddle collision applies timed effect and starts revert coroutine |
| **ScoreManager** | Singleton; tracks `score` and `lives`; exposes `AddScore(int)` and `LoseLife()`; broadcasts `OnScoreChanged` and `OnLivesChanged` events; calls `GameManager.OnGameOver()` when lives reach zero |
| **UIManager** | Subscribes to ScoreManager events; updates UI Toolkit labels for score, lives, level; shows/hides menu screens |

## Communication Patterns

Define how scripts talk to each other. Establish this once and enforce it.

| From | To | Method | Notes |
|---|---|---|---|
| BrickManager | GameManager | C# event — `OnAllBricksCleared` | GameManager subscribes and advances to next level or triggers win |
| Brick | PowerUpManager | Direct method call — `PowerUpManager.Instance.TrySpawnDrop(position)` | PowerUpManager rolls and spawns the drop |
| Brick | BrickManager | Direct method call — `BrickManager.Instance.OnBrickDestroyed()` | Decrements remaining Brick counter |
| Brick | ScoreManager | Direct method call — `ScoreManager.Instance.AddScore(points)` | Points value set in Brick ScriptableObject data |
| BallController | ScoreManager | — | No direct link; Ball does not score — Brick does on destruction |
| PaddleController | PowerUpManager | — | PowerUpManager calls into PaddleController to apply/revert size effects |
| ScoreManager | GameManager | Direct method call — `GameManager.Instance.OnGameOver()` | Called when lives reach zero |
| ScoreManager | UIManager | C# event — `OnScoreChanged`, `OnLivesChanged` | UIManager updates HUD labels |
| GameManager | SceneLoader | Direct method call | Scene transitions are always initiated by GameManager |

**Rule**: only the communication methods listed above are permitted. No ad-hoc `Find`/`FindObjectOfType` chains.
