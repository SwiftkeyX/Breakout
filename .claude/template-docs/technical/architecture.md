# Architecture Contract

> The authoritative list of every script in this project, its single responsibility, and how scripts communicate. Fill this in as systems are implemented. Claude reads this before touching any existing script.
>
> For coding conventions and anti-patterns, see `coding-style.md`.
> For which systems are planned and their tier, see `.claude/docs/design/systems-design.md`.

## Script Table

| Script | Responsibility |
|---|---|
| *(System name)* | *(Single responsibility — one sentence)* |

## Communication Patterns

Define how scripts talk to each other. Establish this once and enforce it.

| From | To | Method | Notes |
|---|---|---|---|
| *(System A)* | *(System B)* | *(C# event / Direct method call / etc.)* | *(any notes)* |

**Rule**: only the communication methods listed above are permitted. No ad-hoc `Find`/`FindObjectOfType` chains.
