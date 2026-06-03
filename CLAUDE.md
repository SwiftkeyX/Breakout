# CLAUDE.md

This file provides guidance to Claude Code when working with a Unity project using this agentic workflow.

## Working with the Unity Editor

This project is developed through the **coplay MCP tools** — use them to create/modify GameObjects, scripts, materials, and scenes rather than writing raw `.unity` YAML.

Key coplay workflow:
- `get_unity_editor_state` / `list_game_objects_in_hierarchy` — inspect current scene state
- `set_unity_project_root` — required first call if multiple Unity instances are open
- `create_game_object` / `set_property` / `set_transform` — build scene objects
- `execute_script` — run one-shot editor scripts for bulk operations
- `play_game` / `stop_game` — test in Play Mode
- `check_compile_errors` — verify scripts compile before testing
- `save_scene` — always save after changes

## Project-Specific Rules

For implementation tasks, use the phase skill that matches the current pipeline stage:
- `/preproduction-task` — Phase 1: fill out design and technical docs
- `/implement-system` — Phase 2: design GDD and implement a system
- `/beta-task` — Phase 3: feel tuning, bug pass, performance pass, ship

For one-off doc lookups, consult `.claude/docs/index.md`.

## Collaboration Rules

### Memory vs Rules

| Store as | Where | When to use |
|---|---|---|
| **Rule** | `CLAUDE.md` or `.claude/docs/` | Invariant constraints — architecture, coding standards, Claude behavior. Read on demand. |
| **Memory** | `memory/` (project or global) | Contextual facts that evolve. Recalled when relevant. |

### Git & Commits

- Never run `git add .` or stage all files indiscriminately — always stage files explicitly by name.
- Run `git status` and `git diff` before every commit to verify exactly what is going in.
- Create **atomic commits**: one logical change per commit. If a task touches multiple systems, split into separate commits per system.
- Follow **Conventional Commits**: `feat(scope): description`, `fix(scope): description`, `chore(scope): description`, etc.
- Never commit broken code — verify `check_compile_errors` passes before staging.
- Never use `git commit -a`, `git add -A`, or `git push` to main without explicit user instruction.

### Claude Behavior

**State what you used at the start of every response.** Every response must open with an audit header — even short conversational answers. Format:

```
Docs read: [filename or "none"] — [one-sentence reason]
Skills used: [skill names]
```

- **Docs read**: list every `.claude/docs/`, `CLAUDE.md`, or `memory/` file actually read this turn. Write `none` if you read nothing.
- **Skills used**: list any skill or sub-agent invoked this turn. Omit this line entirely if nothing was invoked (avoids noisy `none`).
- Do NOT list rules or hooks — rules are always active and hooks are run by the harness, not by you.
- This header exists so omissions are visible: if a doc should have been read, its absence is the signal.

**Use `template-docs/` as structural reference when creating project docs.** When creating or updating any doc in `.claude/docs/`, read the corresponding file in `.claude/template-docs/` first to follow the correct structure and format. Never write project-specific content into `.claude/template-docs/`.

**Regenerate the project snapshot after add/edit/delete in Unity Editor.** Call `GenerateProjectSnapshot.Execute()` via `execute_script` after: adding/removing GameObjects in any scene, adding/removing scripts in `Assets/Scripts/`, adding/removing prefabs or audio assets.

**Verify before changing position.** When a user challenges a factual or technical claim:
1. Acknowledge the disagreement
2. Test or verify immediately (run the tool, check the schema, read the docs)
3. Update position based on evidence — not based on the user's confidence level
