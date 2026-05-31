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

Read these docs before implementing features or making design decisions:

- `.claude/docs/design/game-vision.md` — full creative vision (genre, pillars, mechanics, art/audio direction)
- `.claude/docs/design/design-decisions.md` — locked decisions, canonical terminology, spatial constraints
- `.claude/docs/design/systems-design.md` — every system, its responsibility, dependencies, and tier
- `.claude/docs/design/technical-design/` — per-system detailed GDDs (copy `_template.md` for each system)
- `.claude/docs/technical/technical-preferences.md` — engine version, platform, performance budgets, testing requirements
- `.claude/docs/technical/best-practices.md` — project-critical patterns + Unity 6 current patterns (read before writing any code)
- `.claude/docs/technical/architecture.md` — script responsibilities, component patterns, input conventions
- `.claude/docs/technical/coding-style.md` — required patterns and anti-patterns
- `.claude/docs/technical/asset-conventions.md` — folder layout, naming rules, import settings
- `.claude/docs/process/known-issues.md` — open/fixed bugs (check before implementing related systems)

## Collaboration Rules

### Memory vs Rules

| Store as | Where | When to use |
|---|---|---|
| **Rule** | `CLAUDE.md` or `.claude/docs/` | Invariant constraints — architecture, coding standards, Claude behavior. Read on demand. |
| **Memory** | `memory/` (project or global) | Contextual facts that evolve. Recalled when relevant. |

### Claude Behavior

**Verify before changing position.** When a user challenges a factual or technical claim:
1. Acknowledge the disagreement
2. Test or verify immediately (run the tool, check the schema, read the docs)
3. Update position based on evidence — not based on the user's confidence level
