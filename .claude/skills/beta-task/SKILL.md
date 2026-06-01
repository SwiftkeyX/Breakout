Phase 3 beta: dispatcher for beta tasks — feel tuning, bug pass, performance pass, and ship. Routes to the right procedure based on the current unchecked pipeline item.

---

## Step 1 — Check pipeline status

Read `.claude/docs/PIPELINE.md`. Find the first unchecked `- [ ]` item under **Phase 3 — Beta**. Report it to the user and confirm before proceeding.

If all Phase 3 items are checked, report "Phase 3 complete." and stop.

---

## Step 2 — Route to the correct sub-procedure

### Feel tuning

**Read:** `.claude/docs/design/game-vision.md` — review the feel pillars and intended player experience.

**Do:** Adjust numeric values on ScriptableObjects and MonoBehaviour Inspector fields (speed, acceleration, bounce, timing, etc.) to match the game-vision feel description. Make one value change at a time, test via `play_game`, iterate.

**Write:** No script files. Only Inspector/ScriptableObject asset values.

**Done when:** The feel matches the vision doc's description. Check off `Feel tuning` in PIPELINE.md.

---

### Bug pass

**Read:** `.claude/docs/process/known-issues.md` — read every open issue.

**Do:** Fix each open bug in priority order:
1. Read the relevant script(s) for the bug
2. Fix the issue
3. Call `check_compile_errors` after each fix
4. Test via `play_game` to confirm the fix
5. Mark the issue as fixed in `known-issues.md` (add `[Fixed]` and the date)

**Write:** The script(s) being fixed. `process/known-issues.md` (mark issues resolved).

**Done when:** All issues in `known-issues.md` are marked fixed. Check off `Bug pass` in PIPELINE.md.

---

### Performance pass

**Read:** `.claude/docs/technical/technical-preferences.md` — review the performance budgets (target frame rate, GC alloc limits, draw call budget).

**Do:**
1. Call `get_worst_cpu_frames` and `get_worst_gc_frames` in the Unity editor
2. Identify scripts that exceed the budgets
3. Fix allocations, cache components, reduce draw calls as needed
4. Re-profile to confirm improvements

**Write:** Scripts being optimized.

**Done when:** Frame rate and GC allocs are within the budgets defined in `technical-preferences.md`. Check off `Performance pass` in PIPELINE.md.

---

### Ship

**Read:** `.claude/docs/process/build-notes.md` — the release checklist and build steps.

**Do:** Follow the checklist in `build-notes.md` line by line:
- Run the final smoke test (`play_game`)
- Verify all PIPELINE.md items are checked
- Build the project following the steps in `build-notes.md`

**Write:** `process/build-notes.md` — check off each release checklist item as it is completed.

**Done when:** All checklist items in `build-notes.md` are checked. Check off `Ship` in PIPELINE.md.

---

## Outputs by task

| Task | Read | Write |
|---|---|---|
| Feel tuning | `design/game-vision.md` | Inspector/ScriptableObject values only |
| Bug pass | `process/known-issues.md` | Affected scripts; `known-issues.md` |
| Performance pass | `technical/technical-preferences.md` | Affected scripts |
| Ship | `process/build-notes.md` | `process/build-notes.md` checklist |

---

## Constraints

- Always read the specified doc before starting the sub-procedure — do not rely on memory
- Never check off a PIPELINE.md item before the relevant test or verification passes
- For the bug pass: fix and test one issue at a time — do not batch fixes without individual tests
