Step-skill: fixes all open bugs in known-issues.md one at a time, verifying each fix before moving on.

---

## Agent

`gameplay-programmer`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/project-snapshot-index.md` | Read | Current scene hierarchy and scripts — quickly locate affected scripts and components |
| `.claude/docs/beta/known-issues.md` | Read + Write | Source of all bugs; mark each resolved |
| Affected `Assets/Scripts/*.cs` | Read + Write | Scripts being fixed |
| `.claude/docs/PIPELINE.md` | Read + Write | Tick item when all bugs are resolved |
| `.claude/rules-for-skill/rule-read-write-unity.md` | Read | Compile check, play/stop, save, snapshot — Unity editor workflow |
| `.claude/rules-for-skill/rule-what-to-do-get-block-by-previous-step.md` | Read | When to call /regress instead of patching |
| `.claude/rules-for-skill/rule-pipeline-progression-update.md` | Read | When and how to tick PIPELINE.md |

---

## Entry Condition

Phase 3 is active. `known-issues.md` must exist.

---

## Steps

1. Read `known-issues.md` — list every open issue in priority order
2. If the Open table is empty (only placeholder row): tick PIPELINE.md `- [x] Bug pass` and report "No open bugs — bug pass complete."
3. For each open issue, one at a time:
   a. Read the relevant script(s)
   b. Fix the issue
   c. Call `check_compile_errors` — fix any errors before continuing
   d. Call `play_game` to confirm the fix; call `stop_game`
   e. Move the issue to the Resolved table in `known-issues.md` with `[Fixed YYYY-MM-DD]`
   f. If the bug is rooted in a prior-stage design decision (not a code-level fix): call `/regress` instead of patching symptoms
4. After all issues are resolved: tick PIPELINE.md `- [x] Bug pass`

---

## Exit Condition / Gate

`known-issues.md` Open table contains no real rows (only placeholder or empty). PIPELINE.md item ticked.

---

## Constraints

- Fix and verify one issue at a time — never batch fixes without individual tests
- Never tick Bug pass before the Open table is clear
