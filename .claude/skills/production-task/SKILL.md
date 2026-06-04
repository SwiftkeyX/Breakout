Phase 2 production orchestrator: enforces Phase 1 completion, then runs two mandatory sub-phases — design all GDDs first, then implement all systems in tier order.

---

## Agent

Orchestrator — routes to sub-skills. See sub-skill agent assignments.

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/PIPELINE.md` | Read | Verify entry condition; find unchecked systems |
| `.claude/rules-for-skill/rule-what-to-do-get-block-by-previous-step.md` | Read | When to call /regress instead of patching |

---

## Entry Condition

Milestone 0 = [x] in PIPELINE.md. If not met, stop: "Phase 2 is locked — complete Phase 1 and check off Milestone 0 first."

---

## Steps

**Step 1 — Entry check**
Read PIPELINE.md. Confirm Milestone 0 = [x]. If not, stop with the message above.

**Step 2 — Sub-phase A: Design All (auto-sequential)**

For every system listed in PIPELINE.md Phase 2 that does NOT yet have a GDD at `.claude/docs/production/technical-design/<SystemName>.md`, run `/design-system` for that system. Work in tier order: Tier 1 → Tier 2 → Tier 3. Continue automatically to the next system after each GDD is written.

After all GDDs exist:

> "All GDDs written. Please review each file in `.claude/docs/production/technical-design/`. Reply 'approved' when all are confirmed — Sub-phase B will not start until you do."

Wait for explicit user approval before proceeding to Sub-phase B.

**Step 3 — Sub-phase B: Code All (auto-sequential)**

For each system in PIPELINE.md Phase 2 that is still unchecked, run `/code-system` for that system. Work in tier order: Tier 1 → Tier 2 → Tier 3. Continue automatically to the next system after each passes its test and is ticked.

After all systems are checked:
- Confirm Milestone 1 is ticked (all Tier 1 + Tier 2 systems done)
- Confirm Milestone 2 is ticked (all systems done)

**Step 4 — Report**
"Phase 2 complete — all systems designed, implemented, and tested."

---

## Exit Condition

Milestone 2 = [x] in PIPELINE.md.

---

## Constraints

- Sub-phase B must not begin before the user explicitly approves all GDDs
