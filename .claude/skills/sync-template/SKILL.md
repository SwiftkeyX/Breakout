# Skill: sync-template

Sync structural improvements from this project back to the original workflow template.

## When to use

Run `/sync-template` after you have made any improvements to:
- `.claude/template-docs/` (structural doc templates)
- `.claude/agents/` (agent definitions)
- `.claude/skills/` (skill definitions)
- `CLAUDE.md` (workflow rules and behavior)

Do NOT run this to sync project-specific docs (game vision, GDDs, PIPELINE, architecture) — those live in `.claude/docs/` and are never synced.

## Steps

1. **Read config** — read `.claude/sync-config.json` to get `template_path`

2. **Diff the three sync-worthy folders** — compare each file between this project and the template:
   - `.claude/template-docs/` (all files)
   - `.claude/agents/` (all files)
   - `.claude/skills/` (all files, including this one)

3. **Diff CLAUDE.md** — compare this project's `CLAUDE.md` with the template's. Identify any additions that are structural workflow improvements (new behavior rules, new doc path references) vs project-specific content (game name, game-specific paths, game-specific rules). Only carry over structural additions.

4. **Present a summary** — show the user:
   - Which files differ (or are new) and what changed
   - What CLAUDE.md additions would be synced
   - Confirmation prompt before writing anything

5. **Apply changes** (after confirmation):
   - Copy changed files from `template-docs/`, `agents/`, `skills/` to the template directory
   - Apply structural CLAUDE.md additions to template's CLAUDE.md

6. **Commit to template repo** — in the template directory:
   - `git add .claude/ CLAUDE.md`
   - `git commit -m "Sync improvements from [project name]"`

## What is NEVER synced

- `.claude/docs/` — all project-specific docs
- `CLAUDE.md` sections referencing this game's specific systems, paths, or mechanics
- `Assets/`, `ProjectSettings/`, or any Unity project files
