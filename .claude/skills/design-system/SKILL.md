Step-skill: creates the technical design GDD for one system. Run by /production-task during Sub-phase A before any code is written.

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/design/systems-design.md` | Read | Verify tier, responsibility, and dependencies |
| `.claude/docs/technical/architecture.md` | Read | Script name and communication pattern |
| `.claude/docs/technical/best-practices.md` | Read | Project-critical patterns the GDD must respect |
| `.claude/template-docs/design/technical-design/_template.md` | Read | Required GDD structure |
| `.claude/docs/design/technical-design/<SystemName>.md` | Read (if exists) + Write | Output GDD |
| `.claude/docs/PIPELINE.md` | Read | Confirm system is a listed Phase 2 item |

---

## Entry Condition

`systems-design.md`, `architecture.md`, and `best-practices.md` must all exist. If any is missing, call `/regress` on that item before proceeding.

---

## Steps

1. Read all input docs in the order listed above
2. Read the existing GDD if it exists — note sections already filled
3. Write `.claude/docs/design/technical-design/<SystemName>.md` following the template structure exactly:
   - Status, Summary, Overview, Player Fantasy
   - Detailed Design: state machine, fields, methods
   - Formulas and constants
   - Edge cases
   - Dependencies (must match `systems-design.md`)
   - Tuning knobs
   - Visual/Audio requirements
   - Acceptance criteria
   - Open questions
4. Leave nothing as a placeholder — if information is unknown, add it to Open Questions and flag it to the user
5. Present the completed GDD to the user for review

**Do not tick PIPELINE.md** — the `/production-task` orchestrator manages that after Sub-phase B.

---

## Exit Condition

GDD file exists with all template sections filled. User has been notified to review it.

---

## Constraints

- This skill designs only — never write code or modify scripts
- Never tick PIPELINE.md — that is handled by `/code-system` after implementation
- On blocking issue: call `/regress`
