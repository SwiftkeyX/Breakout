Targeted fix in the Unity editor — reads the code, explains the suspected root cause and risks to the user, applies the fix, then hands control back to the user to test and confirm. Use this for a single known bug. For fixing all open bugs from `known-issues.md` in batch, use `/open-session-for-fix-all-bug` instead.

---

## Agent

`gameplay-programmer`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/project-snapshot-index.md` | Read | **Mandatory first read** — current scene hierarchy, scripts, and prefabs; locate the affected object/component before touching anything |
| `.claude/docs/beta/known-issues.md` | Write | Remove the bug card from Open after the fix is confirmed by the user |
| `.claude/docs/beta/known-issues-archive.md` | Write | Append full card + fix note after fix is confirmed by the user |
| `.claude/docs/preproduction/architecture.md` | Read | Communication patterns — ensure the fix respects system boundaries |
| `.claude/docs/preproduction/best-practices.md` | Read | Project-critical patterns that override all other decisions |
| `.claude/rules-for-skill/rule-read-write-unity.md` | Read | Compile check, play/stop, save, snapshot — Unity editor workflow |
| `.claude/rules-for-skill/rule-what-to-do-get-block-by-previous-step.md` | Read | When to call /regress instead of patching |

---

## Entry Condition

User has described a specific thing to fix. `project-snapshot-index.md` must exist.

---

## Steps

1. **Read `project-snapshot-index.md`** — locate the affected GameObject, script, or asset before writing any code or calling any MCP tools
2. **Read `architecture.md` and `best-practices.md`** — confirm the fix won't violate system boundaries or coding standards
3. **Inspect live state** — call `get_unity_editor_state` and `get_game_object_info` on the affected object to verify current component values match the snapshot
4. **Diagnose and report** — before touching anything, tell the user:
   - What you found in the code (the suspected root cause, with file + line reference)
   - What the fix will change and why
   - What could go wrong or what edge cases the fix might miss
   End with: "**Apply the fix?**" — wait for explicit approval before touching anything.
5. **Apply the fix** — edit the script file or use `set_property` / `set_transform` / `add_component` / `remove_component` as appropriate
   - One logical change at a time — do not bundle unrelated fixes
6. **Compile check** — call `check_compile_errors`; fix all errors before continuing
7. **Hand off to user** — tell the user exactly what to do to test the fix and what to look for:
   - The specific action to take in Play Mode (e.g. "destroy all bricks in Level 1")
   - The exact outcome that confirms the fix worked (e.g. "Level 2 should load automatically")
   - The exact outcome that means it failed (e.g. "level sits empty with no transition")
   Do NOT call `play_game` yourself. Wait for the user to test and report back.
8. **Confirm with user** — ask: "Did it work? (yes / no / something else happened)"
   - **yes** → proceed to Step 9
   - **no / something else** → diagnose further; do not archive yet
9. **Save** — call `save_scene`
10. **Archive the bug** — if this bug came from `known-issues.md`: remove its card from the Open section of `known-issues.md`, then append the full card (all original fields + "Fixed in: YYYY-MM-DD — <one-line fix description>") to `known-issues-archive.md` (add a summary table row + paste the full `### #N` card below the table).
11. **Update snapshot** — run `GenerateProjectSnapshot.Execute()` via `execute_script` if any GameObjects, scripts, or prefabs were added or removed

---

## Exit Condition

User confirms the fix works. No compile errors. Scene saved.

---

## Constraints

- Never apply a fix before inspecting current state in Steps 1–3
- Never apply a fix before reporting diagnosis to the user and receiving approval (Step 4)
- Never call `play_game` to self-test — hand off to the user to test and confirm
- Never archive the bug until the user explicitly confirms it worked (Step 8)
