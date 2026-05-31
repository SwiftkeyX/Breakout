# Unity Agentic Workflow Template

A Claude Code template for building Unity games with AI assistance via the **coplay MCP** tools.

## What's Included

### Core instructions
| Path | Purpose |
|---|---|
| `CLAUDE.md` | Root instructions — coplay workflow, doc index, Claude behavior |
| `docs/PIPELINE.md` | Phase tracker — Pre-production → Production → Beta |

### Design docs (`docs/design/`)
| Path | Purpose |
|---|---|
| `docs/design/game-vision.md` | Full creative vision — genre, pillars, mechanics, art/audio |
| `docs/design/design-decisions.md` | Locked decisions — terminology, constraints, Claude-facing rules |
| `docs/design/systems-design.md` | Every system, its responsibility, dependencies, and tier |
| `docs/design/technical-design/_template.md` | Per-system GDD template — copy for each system |

### Technical docs (`docs/technical/`)
| Path | Purpose |
|---|---|
| `docs/technical/technical-preferences.md` | Engine version, platform, performance budgets, testing requirements |
| `docs/technical/best-practices.md` | Project-critical patterns + Unity 6 current patterns |
| `docs/technical/architecture.md` | Script responsibilities, component patterns |
| `docs/technical/coding-style.md` | Required patterns and anti-patterns |
| `docs/technical/asset-conventions.md` | Folder layout, naming rules, import settings |

### Process docs (`docs/process/`)
| Path | Purpose |
|---|---|
| `docs/process/onboarding.md` | Setup guide — how to open, run, and test the project |
| `docs/process/build-notes.md` | Unity version, platforms, build steps, release checklist |
| `docs/process/known-issues.md` | Open/fixed bugs — check before implementing related systems |
| `docs/process/changelog.md` | Milestone log |

### Agents
| Path | Purpose |
|---|---|
| `.claude/agents/gameplay-programmer.md` | Sub-agent that implements features in Unity via coplay |
| `.claude/agents/technical-director.md` | Sub-agent that reviews architecture (read-only) |

### Architecture Decision Records
| Path | Purpose |
|---|---|
| `docs/adr/` | One file per significant technical decision |

---

## How to Use This Template

### 1. Clone and open in Claude Code

```
git clone <this-repo> my-game
cd my-game
claude
```

### 2. Fill in the design documents

Before writing any code, fill in these files in order:

1. `docs/design/game-vision.md` — title, genre, pillars, mechanics, art/audio direction
2. `docs/design/design-decisions.md` — canonical terms, spatial boundaries, locked decisions
3. `docs/design/systems-design.md` — every system, tier, responsibility, and dependencies
4. `docs/technical/technical-preferences.md` — engine version, platform, performance budgets
5. `docs/technical/architecture.md` — your script table and any extra patterns

Run `/pipeline` at any time to see current progress and the next action.

### 3. Connect to Unity via coplay

Install the [coplay MCP](https://coplay.dev) and open your Unity project. Claude will use coplay tools to create GameObjects, write scripts, and test in Play Mode — no manual YAML editing needed.

### 4. Add per-system design docs

For each major system, copy `docs/design/technical-design/_template.md` and fill it in before implementing that system.

### 5. Start building

Ask Claude to implement features. The `gameplay-programmer` sub-agent handles implementation; the `technical-director` sub-agent handles architecture review.

---

## Prerequisites

- Unity 6 (URP)
- Claude Code CLI
- coplay MCP installed and connected to Unity
