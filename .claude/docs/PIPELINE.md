# PIPELINE.md

## Phase 1 — Pre-production

- [x] Fill out `.claude/docs/design/game-vision.md`
- [x] Fill out `.claude/docs/design/design-decisions.md`
- [x] Fill out `.claude/docs/technical/technical-preferences.md` (engine, platform, performance budgets)
- [x] Fill out `.claude/docs/design/systems-design.md` — list every system, tier, and dependencies
- [x] Fill out `.claude/docs/technical/architecture.md` with finalized script table
- [x] Fill out `.claude/docs/technical/best-practices.md` — add project-critical patterns section
- [x] Milestone 0 — vision complete, all systems tiered, architecture and tech stack finalized

## Phase 2 — Production

### Tier 1 — Foundation
- [x] Create `.claude/docs/design/technical-design/<system>.md` for each Tier 1 system (copy `_template.md`)
- [x] GameManager — game state enum, lives, level index
- [x] SceneLoader — async scene transitions
- [x] InputHandler — mouse position → paddle target X

### Tier 2 — Core Loop
- [x] Create `.claude/docs/design/technical-design/<system>.md` for each Tier 2 system (copy `_template.md`)
- [x] PaddleController — movement, bounds, size changes
- [x] BallController — velocity, bounce, speed clamp
- [x] BrickManager — grid spawn, remaining count, win event
- [x] Brick — HP, VFX, PowerUp drop roll

- [x] Milestone 1 — core loop playable end-to-end

### Tier 3 — Supporting Systems
- [ ] Create `.claude/docs/design/technical-design/<system>.md` for each Tier 3 system (copy `_template.md`)
- [ ] PowerUpManager — falling drops, pickup, timed effects
- [ ] ScoreManager — score, lives, events
- [ ] UIManager — HUD, menus (UI Toolkit)

- [ ] Milestone 2 — all features in, content complete

## Phase 3 — Beta

- [ ] Juice pass — screen shake, particles, hit-stop, SFX, music, UI animations
- [ ] Feel tuning — tweak values via ScriptableObjects/Inspector
- [ ] Difficulty tuning — curve, pacing, escalation
- [ ] Bug pass — all known issues fixed (`.claude/docs/process/known-issues.md` clear)
- [ ] Performance pass — GC allocs and frame rate within budgets (`.claude/docs/technical/technical-preferences.md`)
- [ ] Ship — final build, smoke test, release (`.claude/docs/process/build-notes.md` checklist)
