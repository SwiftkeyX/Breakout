# Systems Design

> List every system in the game, its single responsibility, what it depends on, and which tier it belongs to. Fill this out as part of Phase 1 before writing any code.

## Systems Table

| System | Responsibility | Depends On | Tier |
|---|---|---|---|
| GameManager | Owns game state enum and core game data (singleton) | — | 1 |
| SceneLoader | Handles all scene transitions | GameManager | 1 |
| InputHandler | Reads raw mouse position and exposes paddle target X to other systems | — | 1 |
| PaddleController | Moves Paddle horizontally to match target X, enforces play area bounds, handles size changes from PowerUps | InputHandler | 2 |
| BallController | Manages Ball velocity, angle calculation after bounce, composed speed (base × level multiplier × power-up modifier), minimum speed clamp | PaddleController | 2 |
| BrickManager | Spawns the Brick grid for the current level, tracks remaining destructible Brick count, fires win event when count reaches zero, and owns all brick hit/death effects (camera, particles, SFX, score award, PowerUp drop) | GameManager, CameraEffects, ParticlePool, AudioManager, ScoreManager, PowerUpManager | 2 |
| Brick | Tracks its own hit points and damage-color, then reports each hit/death up to BrickManager — owns no effects, scoring, or drops itself | BrickManager | 2 |
| PowerUpManager | Moves falling PowerUps, detects Paddle pickup collision, applies timed effects to BallController / PaddleController, reverts effects on expiry | BallController, PaddleController | 3 |
| ScoreManager | Tracks current score and remaining lives, broadcasts changes for UI, signals GameManager on lives = 0 | GameManager | 3 |
| UIManager | Drives HUD (score, lives, level number) and all menu screens (MainMenu, GameOver) | ScoreManager, GameManager | 3 |

## Tier Definitions

| Tier | Label | Must work before… |
|---|---|---|
| 1 | Foundation | Any gameplay can be tested |
| 2 | Core Loop | Win/lose is reachable end-to-end |
| 3 | Supporting | Content is complete and game is shippable |
