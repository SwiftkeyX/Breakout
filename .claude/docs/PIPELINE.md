# PIPELINE.md

## Phase 1 — Pre-production

**Entry:** Always open (first stage).
**Exit:** Milestone 0 = [x] — all 6 foundation docs filled.
**Skill:** `/preproduction-task`

- [x] Fill out `.claude/docs/design/game-vision.md`
- [x] Fill out `.claude/docs/design/design-decisions.md`
- [x] Fill out `.claude/docs/technical/technical-preferences.md` (engine, platform, performance budgets)
- [x] Fill out `.claude/docs/design/systems-design.md` — list every system, tier, and dependencies
- [x] Fill out `.claude/docs/technical/architecture.md` with finalized script table
- [x] Fill out `.claude/docs/technical/best-practices.md` — add project-critical patterns section
- [x] Milestone 0 — vision complete, all systems tiered, architecture and tech stack finalized

## Phase 2 — Production

**Entry:** Milestone 0 = [x] — all Phase 1 docs complete.
**Exit:** Milestone 2 = [x] — all systems designed, implemented, and tested.
**Skill:** `/production-task`

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
- [x] Create `.claude/docs/design/technical-design/<system>.md` for each Tier 3 system (copy `_template.md`)
- [x] PowerUpManager — falling drops, pickup, timed effects
- [x] ScoreManager — score, lives, events
- [x] UIManager — HUD, menus (UI Toolkit)

- [x] Milestone 2 — all features in, content complete

## Phase 3 — Beta

**Entry:** Milestone 2 = [x] — all production systems complete.
**Exit:** Ship gate passed — all Phase 3 items checked.
**Skill:** `/beta-task`

- [x] Juice pass — screen shake, particles, hit-stop, SFX, music, UI animations
- [ ] Feel tuning — tweak values via ScriptableObjects/Inspector
  - **Gate:** User confirms feel matches the Chaotic · Explosive · Fun pillars in `game-vision.md`
- [ ] Difficulty tuning — curve, pacing, escalation
  - **Gate:** User confirms the difficulty curve feels intentional and escalates correctly across all levels
- [ ] Bug pass — all known issues fixed (`.claude/docs/process/known-issues.md` clear)
  - **Gate:** `known-issues.md` Open table contains no rows (only the placeholder or empty)
- [ ] Performance pass — GC allocs and frame rate within budgets (`.claude/docs/technical/technical-preferences.md`)
  - **Gate:** `get_worst_gc_frames` shows zero alloc in steady-state gameplay; all frames under 16.6ms
- [ ] Ship — final build, smoke test, release (`.claude/docs/process/build-notes.md` checklist)
  - **Gate:** All other Phase 3 items checked AND `build-notes.md` release checklist fully ticked
