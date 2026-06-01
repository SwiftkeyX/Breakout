# Project Snapshot Index

_Generated: 2026-06-01 12:00 — run `GenerateProjectSnapshot.Execute()` to refresh._

> Regenerate after: adding/removing GameObjects in any scene, adding/removing scripts, prefabs, or audio assets.

## Scenes & Hierarchies

### Assets/Scenes/Game.unity


- Main Camera [Camera, AudioListener, UniversalAdditionalCameraData, CameraEffects]
- Global Volume [Volume]
- InputHandler [InputHandler]
- PlayAreaBounds
  - WallLeft [BoxCollider2D]
  - WallRight [BoxCollider2D]
  - WallTop [BoxCollider2D]
  - DeathZone [BoxCollider2D]
- BrickManager [BrickManager]
- Paddle [SpriteRenderer, BoxCollider2D, Rigidbody2D, PaddleController]
- Ball [SpriteRenderer, CircleCollider2D, Rigidbody2D, BallController]
- UIRoot [UIDocument, UIManager]
- PowerUpManager [PowerUpManager]
- GameManager [GameManager, ScoreManager, AudioManager, ParticlePool]

### Assets/Scenes/GameOver.unity


- Main Camera [Camera, AudioListener, UniversalAdditionalCameraData]
- UIRoot [UIDocument, UIManager]

### Assets/Scenes/MainMenu.unity


- Main Camera [Camera, AudioListener, UniversalAdditionalCameraData]
- GameManager [GameManager, ScoreManager, AudioManager, ParticlePool]
- UIRoot [UIDocument, UIManager]

## Scripts (`Assets/Scripts/`)

- **AudioManager** — `Assets/Scripts/AudioManager.cs`
- **BallController** — `Assets/Scripts/BallController.cs`
- **Brick** — `Assets/Scripts/Brick.cs`
- **BrickData** — `Assets/Scripts/BrickData.cs`
- **BrickManager** — `Assets/Scripts/BrickManager.cs`
- **BrickType** — `Assets/Scripts/BrickType.cs`
- **CameraEffects** — `Assets/Scripts/CameraEffects.cs`
- **GameManager** — `Assets/Scripts/GameManager.cs`
- **InputHandler** — `Assets/Scripts/InputHandler.cs`
- **LevelData** — `Assets/Scripts/LevelData.cs`
- **PaddleController** — `Assets/Scripts/PaddleController.cs`
- **ParticlePool** — `Assets/Scripts/ParticlePool.cs`
- **PowerUp** — `Assets/Scripts/PowerUp.cs`
- **PowerUpManager** — `Assets/Scripts/PowerUpManager.cs`
- **PowerUpType** — `Assets/Scripts/PowerUpType.cs`
- **SceneLoader** — `Assets/Scripts/SceneLoader.cs`
- **ScoreManager** — `Assets/Scripts/ScoreManager.cs`
- **UIManager** — `Assets/Scripts/UIManager.cs`

## Prefabs (`Assets/Prefabs/`)

- **Ball** — `Assets/Prefabs/Ball.prefab`
- **Brick** — `Assets/Prefabs/Brick.prefab`
- **Paddle** — `Assets/Prefabs/Paddle.prefab`
- **PowerUp** — `Assets/Prefabs/PowerUp.prefab`

## Audio (`Assets/Audio/`)

**SFX/**
- SfxBallLost.wav — `Assets/Audio/SFX/SfxBallLost.wav`
- SfxBrickBreak.wav — `Assets/Audio/SFX/SfxBrickBreak.wav`
- SfxHitBrick.wav — `Assets/Audio/SFX/SfxHitBrick.wav`
- SfxHitPaddle.wav — `Assets/Audio/SFX/SfxHitPaddle.wav`
- SfxHitWall.wav — `Assets/Audio/SFX/SfxHitWall.wav`
- SfxLevelClear.wav — `Assets/Audio/SFX/SfxLevelClear.wav`
- SfxPowerUp.wav — `Assets/Audio/SFX/SfxPowerUp.wav`

