Step-skill: adds all juice elements — screen shake, particles, hit-stop, SFX, music, UI animations. First step in Phase 3 beta.

---

## Agent

| Category | Agent |
|---|---|
| Screen shake, Particles, Hit-stop, UI animations | `gameplay-programmer` |
| SFX, Music | `audio-engineer` |

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/project-snapshot-index.md` | Read | Current scene hierarchy, scripts, and assets — understand what exists before adding juice |
| `.claude/docs/design/game-vision.md` | Read | Feel pillars and intended player experience to guide juice direction |
| `.claude/docs/technical/technical-preferences.md` | Read | Performance budgets — juice must not exceed frame rate or GC limits |
| `.claude/docs/PIPELINE.md` | Read + Write | Tick item on completion |

---

## Entry Condition

Milestone 2 = [x]. `game-vision.md` and `technical-preferences.md` must exist.

---

## Steps

1. Read `game-vision.md` — note the feel pillars and player experience description
2. Read `technical-preferences.md` — note the frame rate and GC alloc budget
3. Implement each juice category below in order. After each: call `check_compile_errors`, then `play_game` to verify, then `stop_game` before moving on:

   | Category | What to implement |
   |---|---|
   | Screen shake | On ball-brick impact and ball-paddle impact |
   | Particles | Brick destruction VFX; power-up pickup burst |
   | Hit-stop | Brief time-scale dip (≤ 0.1s) on high-impact hits |
   | SFX | Ball bounce, brick hit, brick destroy, power-up pickup, game over, level clear |
   | Music | Looping background track; intensity variant or layer change when appropriate |
   | UI animations | Score pop, lives-lost flash, level transition |

4. After all categories are in, run a full `play_game` feel pass — confirm the juice enhances the feel pillars, not fights them. Call `stop_game`.
5. Call `save_scene`
6. Tick PIPELINE.md: `- [x] Juice pass`

---

## Exit Condition

All 6 juice categories implemented and tested. Full play pass confirms feel alignment with `game-vision.md`. PIPELINE.md item ticked.

---

## Constraints

- Each category must compile-check and pass `play_game` before moving to the next
- Check `get_worst_gc_frames` after adding particles — must stay within budget
- On blocking issue: call `/regress`
