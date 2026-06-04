# Claude Behavior

## For debugging purpose State what docs, skill, agent you used at the start of every response
**State what you used at the start of every response.** Every response must open with an audit header — even short conversational answers. Format:

```
Docs read: [filename or "none"] — [one-sentence reason]
Docs written: [filename] — [one-sentence reason]
Skills used: [skill names]
Agent assigned: [subagent_type — task description]
```

- **Docs read**: list every `.claude/docs/`, `CLAUDE.md`, or `memory/` file actually read this turn. Write `none` if you read nothing.
- **Docs written**: list every `.claude/docs/`, `CLAUDE.md`, or `memory/` file actually written or edited this turn. Omit this line entirely if nothing was written.
- **Skills used**: list any skill or sub-agent invoked this turn. Omit this line entirely if nothing was invoked (avoids noisy `none`).
- **Agent assigned**: list any sub-agent spawned via the Agent tool this turn, including its type and a brief task description. Omit this line entirely if no agent was spawned.
- Do NOT list rules or hooks — rules are always active and hooks are run by the harness, not by you.
- This header exists so omissions are visible: if a doc should have been read, its absence is the signal.

## Read .claude/template-docs/ when creating docs
**Use `.claude/template-docs/` as structural reference when creating project docs.** When creating or updating any doc in `.claude/docs/`, read the corresponding file in `.claude/template-docs/` first to follow the correct structure and format. Never write project-specific content into `.claude/template-docs/`.

**Report broken reference paths immediately.** Whenever you encounter a path reference in any `.md` file (e.g. `[text](path)`, inline code paths, or plain file paths) that does not resolve to an existing file or directory, stop and report it to the user before continuing. Include: the file containing the broken reference, the broken path, and what you expected to find there. Do not silently skip or work around broken paths.

**Verify before changing position.** When a user challenges a factual or technical claim:
1. Acknowledge the disagreement
2. Test or verify immediately (run the tool, check the schema, read the docs)
3. Update position based on evidence — not based on the user's confidence level
