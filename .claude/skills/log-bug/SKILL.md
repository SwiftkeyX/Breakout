Lightweight bug logger: captures one bug immediately without derailing the current session. Writes a structured row to known-issues.md and hands control back. Does NOT fix the bug — that is /bug-pass's job.

---

## Agent

`claude`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/process/known-issues.md` | Read + Write | Append the new bug to the Open table |

---

## Steps

**Step 1 — Gather info**

Ask the user in one message (do not ask field-by-field):

> "Describe the bug:
> 1. What happened? (one sentence)
> 2. Steps to reproduce (numbered list)
> 3. Severity: Blocker / Major / Minor / Cosmetic"

Wait for the user's response before proceeding.

**Step 2 — Assign issue number**

Read `known-issues.md`. Count the real rows in the Open, Fixed, and Won't Fix tables (ignore placeholder rows). The new issue number = total real rows + 1.

**Step 3 — Append to Open table**

Add a new row to the Open table:

```
| N | <description> | <steps to reproduce> | <area — infer from description> | <severity> |
```

Replace the placeholder row `*(no issues yet)*` if it is still present.

**Step 4 — Confirm and resume**

Report: "Bug #N logged. Resume what you were doing — run `/bug-pass` when ready to fix it."

---

## Constraints

- Never attempt to fix the bug — log only
- Never ask for information you can infer (area, date) — only ask the three fields above
- Keep the response after logging to one line
