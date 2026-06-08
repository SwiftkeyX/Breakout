# Collaboration Rules

## Memory vs Rules

| Store as | Where | When to use |
|---|---|---|
| **Rule** | `CLAUDE.md` or `.claude/docs/` | Invariant constraints — architecture, coding standards, Claude behavior. Read on demand. |
| **Memory** | `memory/` (project or global) | Contextual facts that evolve. Recalled when relevant. |

## Design-First Changes

Before changing any game mechanic, power-up type, tuning value, system responsibility, or feature:

1. Run `/design-change` to update the relevant design doc first
2. Only after the doc is updated, implement in code using the appropriate phase skill or `/fix-bug-right-now`

The doc is the source of truth. Code that diverges from a doc is a bug — not the other way around — unless a deliberate design drift has been formally recorded via `/design-change`.

**Routing table — which doc owns which change:**

| Change type | Doc to edit |
|---|---|
| Power-up type, effect, duration, color | `.claude/docs/production/technical-design/power-up-manager.md` |
| Ball / paddle / brick mechanic constraint | `.claude/docs/preproduction/design-decisions.md` |
| System responsibility or dependencies | `.claude/docs/preproduction/systems-design.md` |
| Code pattern or best practice | `.claude/docs/preproduction/best-practices.md` |
| Architecture or communication rule | `.claude/docs/preproduction/architecture.md` |

## Communication Pattern Changes

Before adding or changing any cross-system call — a new singleton reference, a new event subscription, a new serialized dependency between two scripts — run `/design-change` with a description of the communication change.

Do not write or modify any `.cs` file until `/design-change` reports "Design updated."

`/design-change` is responsible for: verifying the change fits an established pattern in `architecture.md`, cascading the update to `systems-design.md` and affected GDDs, and confirming the change aligns with the game pillars.

## Git & Commits

All git rules live in `.claude/skills/make-commit-plan/SKILL.md`. Use `/make-commit-plan` to stage and commit changes.
