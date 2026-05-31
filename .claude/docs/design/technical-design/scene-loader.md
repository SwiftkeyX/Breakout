# SceneLoader

> **Status**: Approved
> **Last Updated**: 2026-05-31
> **Implements Pillar**: Fun — instant, reliable scene transitions keep the game feeling snappy

## Summary

SceneLoader is a static utility class that wraps `SceneManager` and owns the scene name constants. It has no state, no MonoBehaviour, no singleton. GameManager is the only caller.

> **Quick reference** — Layer: `Foundation` · Priority: `MVP` · Key deps: `GameManager (caller)`

---

## Overview

Rather than scattering `SceneManager.LoadScene("MainMenu")` calls across the codebase, SceneLoader centralises the scene name strings as constants and provides a minimal API. If a scene is renamed, only this file changes.

## Player Fantasy

Transitions feel instant and reliable — no visible hiccups or incorrect scenes loading.

---

## Detailed Design

### Core Rules

1. SceneLoader is a `static class` — it is never instantiated.
2. Scene names are `public const string` constants — no magic strings anywhere else in the codebase.
3. `Load` uses synchronous `SceneManager.LoadScene` for simplicity at Tier 1.
4. `LoadAsync` is available for future use (e.g. loading screen) but is not called at Tier 1.
5. SceneLoader does not decide *when* to load — that decision always lives in GameManager.

### States and Transitions

No state — pure utility class.

### Interactions with Other Systems

| System | Interaction |
|---|---|
| `GameManager` | Sole caller — passes scene name constants into `Load()` |

---

## Formulas

None.

---

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|---|---|---|
| Scene name not in Build Settings | Unity logs an error natively | Not SceneLoader's responsibility to validate |
| `LoadAsync` called but result ignored | Operation still runs in background | Caller is responsible for awaiting if needed |

---

## Dependencies

| System | Direction | Nature |
|---|---|---|
| `UnityEngine.SceneManagement` | This depends on it | Data dependency — wraps SceneManager |
| `GameManager` | It depends on this | State trigger — GameManager calls Load on state change |

---

## Tuning Knobs

None — scene names are constants, not designer-tunable.

---

## Acceptance Criteria

- [ ] All three scene name constants compile without error
- [ ] `SceneLoader.Load(SceneLoader.GAME)` loads the Game scene when called
- [ ] No instance of SceneLoader is ever created (static class enforces this at compile time)
- [ ] No scene name string literals appear anywhere outside `SceneLoader.cs`

---

## Open Questions

None.
