# Architecture Contract

> The authoritative record of how scripts in this project communicate. Fill this in as systems are implemented. Claude reads this before touching any existing script.
>
> For system responsibilities, dependencies, and tier assignments, see `.claude/docs/preproduction/systems-design.md`.
> For coding conventions and anti-patterns, see `.claude/template-docs/technical/coding-style.md`.

## Communication Patterns

Define how scripts talk to each other. Establish this once and enforce it.

| From | To | Method | Notes |
|---|---|---|---|
| BrickManager | GameManager | Direct method call — `GameManager.Instance.OnLevelComplete()` | Called when remaining Brick count reaches zero |
| Brick | BrickManager | Direct method call — `_manager.OnBrickDestroyed(data, position)` / `OnBrickDamaged()` | Brick reports hits/death up; manager is held via `Init()`, not a singleton |
| BrickManager | ScoreManager | Direct method call — `ScoreManager.Instance.AddScore(points)` | Awarded on brick death; point value from `BrickData` |
| BrickManager | PowerUpManager | Direct method call — `TrySpawnDrop(position)` | Manager rolls and spawns the drop on brick death |
| BrickManager | CameraEffects / ParticlePool / AudioManager | Direct method call | Brick hit/death juice — hitstop, shake, particle burst, SFX |
| BallController | ScoreManager | — | No direct link; Ball does not score — BrickManager does on destruction |
| BrickManager | BallController | Direct method call — `SetLevelMultiplier(multiplier)` | Per-level base-speed scaling |
| PowerUpManager | BallController | Direct method call — `SetSpeedModifier(factor)` | Transient SlowBall scaling; composes with the level multiplier |
| PaddleController | PowerUpManager | — | PowerUpManager calls into PaddleController to apply/revert size effects |
| ScoreManager | GameManager | Direct method call — `GameManager.Instance.OnGameOver()` | Called when lives reach zero |
| ScoreManager | UIManager | C# event — `OnScoreChanged`, `OnLivesChanged` | UIManager updates HUD labels |
| GameManager | SceneLoader | Direct method call | Scene transitions are always initiated by GameManager |

**Rule**: only the communication methods listed above are permitted. No ad-hoc `Find`/`FindObjectOfType` chains.
