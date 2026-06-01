# Architecture Contract

> The authoritative record of how scripts in this project communicate. Fill this in as systems are implemented. Claude reads this before touching any existing script.
>
> For system responsibilities, dependencies, and tier assignments, see `.claude/docs/design/systems-design.md`.
> For coding conventions and anti-patterns, see `.claude/template-docs/technical/coding-style.md`.

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
