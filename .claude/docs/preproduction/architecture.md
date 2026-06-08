# Architecture Contract

> The authoritative record of how scripts in this project communicate. Fill this in as systems are implemented. Claude reads this before touching any existing script.
>
> For system responsibilities, dependencies, and tier assignments, see `.claude/docs/preproduction/systems-design.md`.
> For coding conventions and anti-patterns, see `.claude/template-docs/technical/coding-style.md`.

## Communication Patterns

Scripts communicate via three patterns. The choice of pattern follows from the relationship between systems.

---

### C# Events — one publisher, many unknown subscribers

**Use when:** a system needs to broadcast a state change but must not know who is listening.

**GameManager → BallController, PowerUpManager, UIManager**
GameManager fires `OnGameStateChanged` and `OnLivesChanged`. It never imports or references these subscribers — they opt in by subscribing. This keeps GameManager decoupled from every downstream system that reacts to game state.

**BrickManager → CameraEffects, AudioManager**
BrickManager fires `BrickDestroyed`, `BrickDamaged`, and `LevelCleared`. Camera shake, hit-stop, and audio are observers with no business logic — they have no reason to be hardcoded into BrickManager. Event subscription lets them attach and detach cleanly.

**ScoreManager → UIManager**
ScoreManager fires `OnScoreChanged`. UI is a pure display layer; ScoreManager should not reference it.

---

### Direct Method Calls — one caller, one known collaborator

**Use when:** the caller has a specific business contract with one target and needs a synchronous result or guaranteed execution order.

**BrickManager → ScoreManager, PowerUpManager, BallController**
On brick death, BrickManager must award score, attempt a drop, and apply speed scaling — all synchronous, in-order operations with exactly one target each. Direct calls (via serialized refs or singleton) make the contract explicit.

**BrickManager → GameManager** (registration pattern)
`BrickManager.Start()` calls `GameManager.RegisterBrickManager(this)` so GameManager can subscribe to `LevelCleared`. This one-time handshake solves an Awake/Start ordering race: by Start, all Awake calls are complete and `BrickManager.Instance` is guaranteed to exist.

**GameManager → SceneLoader, ScoreManager**
GameManager is the sole authority on scene transitions and game resets. Direct calls make the ownership clear — no other system may load scenes or reset score.

**UIManager → GameManager**
Button callbacks invoke GameManager methods directly. UI drives intent; GameManager owns execution.

---

### Serialized Inspector Injection — tightly coupled pairs

**Use when:** two objects collaborate so closely that loose coupling adds no benefit and a designer needs to wire them in the Inspector.

- `PaddleController` holds a reference to `InputHandler`
- `BallController` holds a reference to `PaddleController`
- `BrickManager` holds references to `BallController` and `PowerUpManager`
- `Brick` receives a `BrickManager` reference via `Init()` on spawn — not the Inspector, but still an injected dependency

---

### Rule

> When adding a new cross-system call: pick the pattern that matches the relationship above. If neither fits cleanly, discuss before implementing — do not use `FindObjectOfType` or static singletons outside the patterns above.
