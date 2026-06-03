Phase 1 pre-production: fill out all design and technical foundation docs from templates, then check off completed items in PIPELINE.md.

---

## Step 1 — Check pipeline status

Read `.claude/docs/PIPELINE.md`. Identify all unchecked `- [ ]` items under **Phase 1 — Pre-production**. If all Phase 1 items are already checked, report "Phase 1 complete." and stop.

List the unchecked items to the user before proceeding.

---

## Step 2 — For each unchecked Phase 1 doc, fill it out

Work through unchecked items in this order:

| PIPELINE.md item | Template to read first | Project doc to write |
|---|---|---|
| Fill out `game-vision.md` | *(no template — original content)* | `.claude/docs/design/game-vision.md` |
| Fill out `design-decisions.md` | *(no template — original content)* | `.claude/docs/design/design-decisions.md` |
| Fill out `technical-preferences.md` | `.claude/template-docs/technical/technical-preferences.md` | `.claude/docs/technical/technical-preferences.md` |
| Fill out `systems-design.md` | `.claude/template-docs/design/systems-design.md` | `.claude/docs/design/systems-design.md` |
| Fill out `architecture.md` | `.claude/template-docs/technical/architecture.md` | `.claude/docs/technical/architecture.md` |
| Fill out `best-practices.md` | `.claude/template-docs/technical/best-practices.md` | `.claude/docs/technical/best-practices.md` |

For each:
1. Read the template file (if one exists) to understand the required structure and sections
2. If the project doc already exists, read it to see what is already filled in
3. Ask the user for any project-specific information needed (game name, engine version, systems list, etc.) before writing
4. Write the completed project doc — follow the template structure exactly, replace all placeholder content with project-specific content
5. Never write project-specific content into `template-docs/`

---

## Step 3 — Check off completed items

After each doc is written, update `.claude/docs/PIPELINE.md`: change `- [ ]` to `- [x]` for that item.

After all docs are filled: check off `Milestone 0 — vision complete, all systems tiered, architecture and tech stack finalized` if all six docs are done.

---

## Step 4 — Report completion

List every doc written and its path. Confirm Phase 1 status: "Phase 1 complete — all items checked."

---

## Constraints

- Never skip reading the template before writing the project doc — structure must match
- Never fill in placeholder values with made-up content — ask the user if information is missing
- Never modify `template-docs/` files
