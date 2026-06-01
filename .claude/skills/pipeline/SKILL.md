Read `.claude/docs/PIPELINE.md` and do the following:

1. **Report status** — identify the current phase (the earliest phase with unchecked items) and show a progress count (e.g. "Phase 1 — Pre-production: 2/6 done").

2. **Show next action** — surface the first unchecked item and give a concrete instruction on how to complete it. Be specific: name the file to open, the section to fill in, or the tool to run.

3. **Offer to advance** — ask the user if any items are done and should be checked off. If yes, update the checkbox in `PIPELINE.md` from `- [ ]` to `- [x]` for each confirmed item.

Keep the response short. No preamble — lead with the status line.
