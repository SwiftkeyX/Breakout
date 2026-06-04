# Design Decisions

> Terminology and constraints that Claude must apply consistently throughout the codebase.
> For core loop, win/lose, and mechanics — those live in `game-vision.md` (the source of truth).
> Add an entry here only when a decision is finalized AND it affects how Claude names or constrains things in code.

## Terminology

Define canonical names so Claude uses them consistently. Every term here overrides any synonym.

| Term | Means | Not |
|---|---|---|
| **Paddle** | The player-controlled bar at the bottom of the play area | not "bar", "bat", "player", "platform" |
| **Ball** | The bouncing projectile the player keeps in play | not "sphere", "projectile", "puck" |
| **Brick** | A destructible tile in the level grid | not "block", "tile", "target", "enemy" |
| **PowerUp** | A collectible that falls from a destroyed Brick and grants a temporary effect | not "pickup", "item", "drop", "buff" |
| **Life** | One attempt unit — lost when the Ball exits the bottom boundary | not "heart", "chance", "health" |
| **Play Area** | The bounded rectangle the Ball moves within | not "arena", "field", "stage" |

## Boundaries & Constraints

Spatial and mechanical limits that affect implementation decisions.

| Constraint | Value | Notes |
|---|---|---|
| Play area bounds | Camera-bounded orthographic rect at 1920×1080 | Ball and Paddle are always fully visible; nothing exits the sides or top |
| Ball bottom exit | Below the Paddle's lower edge = Life lost | Ball is destroyed and respawned on Paddle after brief delay |
| Paddle movement | Horizontal only, along the bottom of the Play Area | Paddle Y-position is fixed; cannot move vertically |
| Paddle boundary | Clamped to left/right edges of the Play Area | Paddle cannot exit the screen |
| Brick grid region | Upper 60% of the Play Area | Bottom 40% reserved for Paddle movement and Ball approach |
| Ball minimum speed | Must be clamped above `minSpeed` after every bounce | Prevents floating-point energy loss from making the Ball crawl |
