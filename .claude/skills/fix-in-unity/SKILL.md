Targeted fix in the Unity editor — script edits, scene wiring, Inspector value corrections, or component changes. Always reads the project snapshot first to ground the fix in current scene state.

---

## Agent

`gameplay-programmer`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/project-snapshot-index.md` | Read | **Mandatory first read** — current scene hierarchy, scripts, and prefabs; locate the affected object/component before touching anything |
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
4. **Apply the fix** — edit the script file or use `set_property` / `set_transform` / `add_component` / `remove_component` as appropriate
   - One logical change at a time — do not bundle unrelated fixes
5. **Compile check** — call `check_compile_errors`; fix all errors before continuing
6. **Test** — call `play_game`, observe the specific behaviour that was broken, call `stop_game`; confirm the fix works
7. **Save** — call `save_scene`
8. **Update snapshot** — run `GenerateProjectSnapshot.Execute()` via `execute_script` if any GameObjects, scripts, or prefabs were added or removed

---

## Exit Condition

Fix verified in Play Mode with no compile errors. Scene saved.

---

## Constraints

- Never apply a fix before inspecting current state in Steps 1–3
