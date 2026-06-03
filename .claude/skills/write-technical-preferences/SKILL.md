Step-skill: writes technical-preferences.md from the template. Third step in the pre-production sequence.

---

## Agent

`claude`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/template-docs/technical/technical-preferences.md` | Read | Required structure and sections |
| `.claude/docs/technical/technical-preferences.md` | Read (if exists) + Write | Output doc |
| `.claude/docs/PIPELINE.md` | Read + Write | Tick item on completion |

---

## Entry Condition

Phase 1 is active. Template must exist at the path above.

---

## Steps

1. Read `.claude/template-docs/technical/technical-preferences.md` — note every required section
2. Read `.claude/docs/technical/technical-preferences.md` if it exists — note sections already filled
3. Ask the user for:
   - Unity version (exact version number)
   - Render pipeline (URP / Built-in / HDRP)
   - Target platform(s)
   - Target frame rate (e.g. 60 fps)
   - GC alloc budget (e.g. zero steady-state)
   - Draw call budget
   - Any other platform-specific constraints
4. Write the project doc following the template structure exactly — no placeholder values
5. Update PIPELINE.md: tick `- [x] Fill out technical-preferences.md`

---

## Exit Condition

`technical-preferences.md` exists with all template sections filled and no placeholder values. PIPELINE.md item ticked.

---

## Constraints

- Never invent performance budgets — ask the user
- Template structure must be followed exactly; never write project content into `template-docs/`
- On blocking issue: call `/regress`
