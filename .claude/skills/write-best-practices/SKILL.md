Step-skill: writes best-practices.md, capturing project-critical coding patterns that override all defaults. Sixth and final step in the pre-production sequence.

---

## Agent

`claude`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/technical/architecture.md` | Read | Communication patterns become hard rules |
| `.claude/template-docs/technical/best-practices.md` | Read | Required structure |
| `.claude/docs/technical/best-practices.md` | Read (if exists) + Write | Output doc |
| `.claude/docs/PIPELINE.md` | Read + Write | Tick item and Milestone 0 on completion |

---

## Entry Condition

`architecture.md` must exist. If missing, call `/regress "Fill out architecture.md" "required before best-practices"`.

---

## Steps

1. Read `architecture.md` and the best-practices template
2. Read `best-practices.md` if it exists — note patterns already defined
3. Derive project-critical rules from the architecture:
   - Communication rules (e.g. "GameManager must never call AudioManager directly — use events")
   - Unity-specific patterns (e.g. "Never use FindObjectOfType at runtime — cache in Awake")
   - Project invariants (e.g. "Ball must never stop moving")
   - Performance rules aligned with `technical-preferences.md` budgets
4. Ask the user if there are additional critical rules before writing
5. Write `.claude/docs/technical/best-practices.md` following the template structure — project-critical section must be prominent
6. Update PIPELINE.md:
   - Tick `- [x] Fill out best-practices.md`
   - If all 6 Phase 1 docs are now checked, tick `- [x] Milestone 0 — vision complete...`

---

## Exit Condition

`best-practices.md` exists with a project-critical patterns section. Milestone 0 ticked in PIPELINE.md.

---

## Constraints

- Every rule in the project-critical section is a hard constraint, not a suggestion
- On blocking issue: call `/regress`
