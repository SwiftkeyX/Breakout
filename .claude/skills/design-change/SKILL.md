Update one or more design docs to reflect a proposed game change. Run this BEFORE touching any code — it is the first mandatory step for any mechanic, feature, or value change.

---

## Agent

`claude`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/preproduction/game-vision.md` | Read | Verify the change aligns with the three pillars |
| `.claude/docs/preproduction/design-decisions.md` | Read / Write if mechanic change | Fundamental mechanic constraints |
| `.claude/docs/preproduction/systems-design.md` | Read / Write if system scope changes | System responsibilities and dependencies |
| `.claude/docs/preproduction/architecture.md` | Read / Write if communication pattern changes | Architecture and communication rules |
| `.claude/docs/preproduction/best-practices.md` | Read / Write if coding pattern changes | Project-critical code patterns |
| `.claude/docs/production/gdd/<System>.md` | Read / Write if system-level detail changes | Per-system GDD: effects, values, types |
| `.claude/rules/collaboration.md` | Read | Routing table for which doc owns which change |

---

## Entry Condition

The user provides a plain-language description of the change they want, e.g.:
- "Replace SpeedBoost with SlowBall"
- "Drop chance should be 30% not 55%"
- "Ball should not bounce off the side walls"

---

## Steps

1. Read `game-vision.md` and `collaboration.md` routing table
2. Identify which doc(s) own the data being changed using the routing table
3. Read those target docs
4. Verify the proposed change does not contradict the three game pillars (Chaotic · Explosive · Fun). If it does, flag the conflict to the user before proceeding
5. Edit the target doc(s) — update tables, tuning knobs, acceptance criteria, or constraint prose as needed. Be surgical: change only what the request covers
   5b. **If `architecture.md` was edited** (communication pattern changed): cascade to these docs in order — do not flag, auto-update:
       - `systems-design.md` — update the "Depends On" entry for every system that gained or lost a connection
       - `gdd/<System>.md` for each affected system — update its "Interactions with Other Systems" section and its "Dependencies" section
6. Report to the user:
   - What was changed and in which doc(s)
   - Which skill to run next to implement the change in code (e.g. `/fix-bug-right-now`, `/code-system`, `/tuning-loop`)

---

## Exit Condition

All target docs updated. User notified with a "Design updated — implement via [skill]" summary.

---

## Constraints

- Never write or edit any `.cs` file
- Never edit `.unity` scenes
- Never call coplay MCP tools
- Never tick PIPELINE.md
- One change at a time — if the request spans multiple unrelated changes, handle them sequentially and confirm between each
