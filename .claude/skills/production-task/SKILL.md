Phase 2 production: complete procedure for one game system — design its GDD if missing, then implement it in Unity. Works for any tier.

---

## Step 1 — Identify the system

Ask the user: "Which system are you implementing?" (e.g., BallController, PaddleController, GameManager).

Check `.claude/docs/PIPELINE.md` to confirm the system is listed as an unchecked Phase 2 item. If not listed, flag it and ask the user to confirm before continuing.

---

## Step 2 — Read context docs (in this order)

Read all four before touching any code or GDD:

1. **`.claude/docs/design/systems-design.md`** — verify the system's tier and which systems it depends on
2. **`.claude/docs/technical/architecture.md`** — note the script name, responsibility, and which components it interacts with
3. **`.claude/docs/technical/best-practices.md`** — note all project-critical patterns; these override everything
4. **`.claude/docs/project-snapshot-index.md`** — understand current scene state: which GameObjects exist, which scripts are already implemented

---

## Step 3 — Design the GDD (if it doesn't exist)

Check whether `.claude/docs/design/technical-design/<SystemName>.md` exists.

**If it does not exist:**
1. Read `.claude/template-docs/design/technical-design/_template.md`
2. Create `.claude/docs/design/technical-design/<SystemName>.md` — fill every section using the template structure; leave nothing as a placeholder
3. Ask the user to review and confirm the GDD before implementation begins

**If it already exists:** read it fully before proceeding.

---

## Step 4 — Implement

Use the gameplay-programmer agent or coplay MCP tools directly. Follow this sequence:

1. **Verify editor state** — call `get_unity_editor_state` and `list_game_objects_in_hierarchy`
2. **Check existing scripts** — call `list_files` on `Assets/Scripts/` to see what is already implemented
3. **Write script(s)** — write `Assets/Scripts/<SystemName>.cs` (and any supporting scripts listed in the GDD) to disk
   - One script per responsibility — never merge two responsibilities
   - Apply every pattern from `best-practices.md` exactly
4. **Compile check** — call `check_compile_errors` after every script write; fix all errors before continuing
5. **Wire scene** — place GameObjects, assign component references, set transforms per the GDD
6. **Save** — call `save_scene`
7. **Test** — call `play_game`, observe Unity console logs, call `stop_game`; fix any issues, then re-test

---

## Step 5 — Check off PIPELINE.md

After a successful test:
1. Update `.claude/docs/PIPELINE.md`: change `- [ ] <SystemName>` to `- [x] <SystemName>`
2. If all systems in a tier are now checked, also check off that tier's milestone line

---

## Step 6 — Report

State:
- GDD path written (if new)
- Script(s) written: `Assets/Scripts/<SystemName>.cs`
- Scene changes made
- PIPELINE.md items checked off

---

## Outputs

| What | Path |
|---|---|
| System GDD (if new) | `.claude/docs/design/technical-design/<SystemName>.md` |
| Primary script | `Assets/Scripts/<SystemName>.cs` |
| Supporting scripts | `Assets/Scripts/<RelatedName>.cs` (as specified in GDD) |
| PIPELINE.md update | `.claude/docs/PIPELINE.md` |

---

## Constraints

- Never start implementing before reading all four context docs in Step 2
- Never leave compile errors unfixed
- Never skip `save_scene`
- Never check off a PIPELINE.md item before the `play_game` test passes
