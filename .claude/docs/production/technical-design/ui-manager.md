# UIManager

> **Status**: Approved
> **Last Updated**: 2026-06-01
> **Implements Pillar**: Fun — clear feedback and fast restarts keep the game immediately replayable

## Summary

UIManager is a per-scene MonoBehaviour that reads the active UIDocument and binds event handlers based on the current scene name. Three layouts: MainMenu (Start button), Game HUD (score/lives/level), GameOver (result, score, replay buttons).

> **Quick reference** — Layer: `Supporting` · Priority: `MVP` · Key deps: `GameManager`, `ScoreManager`, `UIDocument`

---

## Detailed Design

### Core Rules

1. UIManager uses UI Toolkit exclusively — no UGUI Canvas components.
2. One UIDocument + UIManager per scene. UIManager switches behaviour by `SceneManager.GetActiveScene().name`.
3. All bindings happen in `Start()` — after GameManager.Instance and ScoreManager.Instance are guaranteed present.
4. UIManager subscribes to events but never holds a direct Inspector reference to GameManager/ScoreManager (cross-scene safe via static Instance access).
5. HUD updates on `ScoreManager.OnScoreChanged`, `GameManager.OnLivesChanged`, and `GameManager.OnGameStateChanged`.
6. GameOver screen reads final score from `ScoreManager.Instance.Score` at binding time.
7. HUD handlers are cached in fields and detached in `OnDestroy`. This is mandatory: the events live on the persistent (`DontDestroyOnLoad`) GameManager/ScoreManager, so without unsubscribing, every scene load would stack new handlers that fire into the previous scene's destroyed `VisualElement`s.
8. The GameOver "YOU WIN" check reads `GameManager.Instance.TotalLevels` — never a hardcoded level count.

### Layouts

| Scene | UXML | Key Elements |
|---|---|---|
| MainMenu | `MainMenuUI.uxml` | `#title`, `#subtitle`, `#start-button` |
| Game | `GameHUD.uxml` | `#score-label`, `#level-label`, `#lives-label` |
| GameOver | `GameOverUI.uxml` | `#result-label`, `#score-label`, `#restart-button`, `#menu-button` |

### Visual Style

- Background: `rgb(18, 18, 18)` (near-black)
- Primary text: white
- Secondary text: `rgb(160, 160, 160)`
- Buttons: dark fill with subtle border, no rounded corners

---

## Acceptance Criteria

- [ ] MainMenu Start button calls `GameManager.Instance.StartGame()`
- [ ] HUD score label updates immediately on brick destruction
- [ ] HUD lives label updates immediately on ball lost
- [ ] GameOver shows correct final score
- [ ] GameOver shows "YOU WIN" instead of "GAME OVER" when `CurrentLevelIndex >= GameManager.TotalLevels`
- [ ] Play Again and Main Menu buttons work correctly
- [ ] `OnDestroy` detaches every handler added to GameManager/ScoreManager — no leak across scene loads
- [ ] No UGUI Canvas components anywhere

---

## Open Questions
None.
