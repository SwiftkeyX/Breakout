Step-skill: implements one system in Unity from its approved GDD. Run by /production-task during Sub-phase B after all GDDs are approved.

---

## Agent

`gameplay-programmer`

---

## Docs

| Doc | Read/Write | Purpose |
|---|---|---|
| `.claude/docs/production/technical-design/<SystemName>.md` | Read | Approved GDD — implementation source of truth |
| `.claude/docs/preproduction/architecture.md` | Read | Communication patterns and allowed references |
| `.claude/docs/preproduction/best-practices.md` | Read | Project-critical patterns — override everything |
| `.claude/docs/preproduction/technical-preferences.md` | Read | Performance budgets to respect while coding |
| `.claude/docs/project-snapshot-index.md` | Read (if exists) + Write | Current scene state; update after changes |
| `Assets/Scripts/<SystemName>.cs` | Write | Primary output script |
| `.claude/docs/PIPELINE.md` | Read + Write | Tick system item on successful test |
| `.claude/rules-for-skill/rule-read-write-unity.md` | Read | Compile check, play/stop, save, snapshot — Unity editor workflow |
| `.claude/rules-for-skill/rule-what-to-do-get-block-by-previous-step.md` | Read | When to call /regress instead of patching |
| `.claude/rules-for-skill/rule-pipeline-progression-update.md` | Read | When and how to tick PIPELINE.md |

---

## Entry Condition

GDD at `.claude/docs/production/technical-design/<SystemName>.md` must exist and be approved. If missing, call `/regress "Design <SystemName> GDD" "GDD required before coding"`.

---

## Steps

1. Read all input docs in the order listed above
2. **Verify editor state** — call `get_unity_editor_state` and `list_game_objects_in_hierarchy`
3. **Check existing scripts** — call `list_files` on `Assets/Scripts/`
4. **Write script(s)** — write `Assets/Scripts/<SystemName>.cs` per the GDD
   - One script per responsibility — never merge two responsibilities into one file
   - Apply every rule from `best-practices.md` exactly
   - Respect all performance budgets from `technical-preferences.md`
5. **Compile check** — call `check_compile_errors`; fix all errors before continuing
6. **Wire scene** — place GameObjects, assign component references, set transforms per the GDD
7. **Save** — call `save_scene`
8. **Test** — call `play_game`, observe console, call `stop_game`; fix any issues and re-test until clean
9. **Update snapshot** — run `GenerateProjectSnapshot.Execute()` via `execute_script`
10. **Tick PIPELINE.md** — change `- [ ] <SystemName>` to `- [x] <SystemName>`; if all systems in a tier are now checked, tick that tier's milestone line too

---

## Exit Condition

`play_game` test passes with no errors. PIPELINE.md item ticked. Snapshot updated.

---

## Constraints

- Never start coding without reading the GDD first
- Never tick PIPELINE.md before `play_game` passes
