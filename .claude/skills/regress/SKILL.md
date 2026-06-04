Regression trigger: un-checks a PIPELINE.md item, logs the regression in known-issues.md, then re-routes via /check-pipeline-stage. Called by any step-skill that hits a blocking issue rooted in an earlier stage.

---

## Agent

`claude`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/PIPELINE.md` | Read + Write | Find and un-check the named item |
| `.claude/docs/beta/known-issues.md` | Read + Write | Log the regression entry |

---

## Entry Condition

Can be called from any stage. No pipeline state prerequisite — regression is always valid if justified.

---

## Steps

**Input required:** item name (the PIPELINE.md item text, or a unique keyword) and reason (why this stage must be revisited).

1. **Read** `.claude/docs/PIPELINE.md`
2. **Find** the line matching the item name — if no unique match, report the ambiguity and ask the user to clarify before proceeding
3. **Un-check** it: change `- [x]` → `- [ ]` on that line only
4. **Write** the updated PIPELINE.md
5. **Read** `.claude/docs/beta/known-issues.md`
6. **Add** a row to the Open table:

   | Date | Item | Reason | Regressed from |
   |------|------|--------|----------------|
   | YYYY-MM-DD | `<item name>` | `<reason>` | Phase N |

7. **Write** the updated known-issues.md
8. **Report** to the user: "Regression logged — `<item>` re-opened. Reason: `<reason>`."
9. **Run** `/check-pipeline-stage` to detect the unchecked item and re-route to the correct orchestrator

---

## Exit Condition

Regression is complete when:
- PIPELINE.md item is un-checked
- known-issues.md entry is written
- `/check-pipeline-stage` has been run and has reported the new route

---

## Constraints

- Never un-check an item without a stated reason
- Never skip the known-issues.md log — regressions must be traceable
- Only un-check one item per call — if multiple items need re-opening, call `/regress` once per item
- Never un-check Milestone items directly — un-check the specific system or doc item that failed; the milestone will automatically become incomplete
