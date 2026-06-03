Review all pending changes, group them into logical atomic commits, present the plan for approval, then stage and commit each group using Conventional Commits format.

---

## Agent

`claude`

---

## Step 1 — Audit current state

Run `git status` and `git diff --stat` to list all modified and untracked files.

If there is nothing to commit → report "Nothing to commit." and stop.

---

## Step 2 — Read each change

For each **modified** file: run `git diff <file>` to understand the nature and scope of the change.

For each **untracked** file: read the file directly to understand its content.

---

## Step 3 — Group into atomic commits

Apply these rules in order to assign every file to a commit group:

1. **Same system/feature** — files that implement or extend the same gameplay system belong together (e.g., `BallController.cs` + `BallConfig.cs` + `Ball.prefab`)
2. **Scripts travel with their scene/prefab** — a `.cs` change and the `.unity` or `.prefab` that wires it belong in the same commit
3. **Docs are isolated** — `.md` changes go in their own commit unless the file is the GDD/spec directly describing the feature being committed
4. **Config/settings are isolated** — `CLAUDE.md`, `.claude/settings.json`, `.claude/skills/`, `ProjectSettings/` changes = a separate `chore:` commit
5. **Split when ambiguous** — prefer smaller atomic commits over bundling; when grouping is genuinely unclear, ask the user before proceeding

---

## Step 4 — Draft commit messages

Format: `type(scope): short imperative description`

| Type | When to use |
|---|---|
| `feat` | New gameplay behavior, new system, new mechanic |
| `fix` | Bug correction |
| `refactor` | Restructure without behavior change |
| `chore` | Tooling, config, workflow, settings |
| `docs` | Documentation only |

Scope = system or area in lowercase (e.g., `ball`, `paddle`, `ui`, `audio`, `config`, `workflow`)

Examples: `feat(ball): add velocity cap and bounce angle`, `chore(workflow): add smart commit skill`, `docs(gdd): update brick system spec`

---

## Step 5 — Present the commit plan

Before touching anything, display a table:

```
| # | Commit message                              | Files                              |
|---|--------------------------------------------|------------------------------------|
| 1 | feat(ball): add velocity cap               | BallController.cs, BallConfig.cs   |
| 2 | chore(workflow): add git commit rules      | CLAUDE.md, .claude/settings.json   |
```

Then ask:

> **Proceed with this plan?**
> Reply: `yes` / `edit N <describe change>` (modify commit N's grouping or message) / `cancel`

Apply any edits the user requests, re-show the updated table, and ask again before executing.

---

## Step 6 — Execute each commit in sequence

For each group, in order:

1. **Compile check** — if the group contains any `.cs` files, run `check_compile_errors`. If errors exist, abort the entire sequence and report which errors must be fixed first.
2. **Stage explicitly** — `git add <file1> <file2> ...` using exact file paths, never `git add .` or `git add -A`
3. **Commit** — `git commit -m "message"` using a heredoc for multi-line messages
4. **Confirm** — output: `✓ Committed: feat(ball): add velocity cap`

---

## Step 7 — Final summary

Run `git log --oneline -5` and display the output so the user can verify what landed.

---

## Constraints

- **Never** use `git add .` / `git add -A` / `git commit -a` / `git commit --amend`
- **Never** push unless the user explicitly asks after this skill completes
- **Abort** the entire sequence if compile errors are found — report the errors clearly
- **Ask the user** when grouping is ambiguous — never guess
- **One commit at a time** — complete Step 6 fully for each group before moving to the next
