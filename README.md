# Unity Agentic Workflow Template

A Claude Code template for building Unity games with AI assistance via the **coplay MCP** tools.

## What's Included

### Core instructions
| Path | Purpose |
|---|---|
| `CLAUDE.md` | Root instructions — coplay workflow, doc index, Claude behavior |
| `.claude/docs/PIPELINE.md` | Phase tracker — Pre-production → Production → Beta |

### Pre-production docs (`.claude/docs/preproduction/`)
| Path | Purpose |
|---|---|
| `.claude/docs/preproduction/game-vision.md` | Full creative vision — genre, pillars, mechanics, art/audio |
| `.claude/docs/preproduction/design-decisions.md` | Locked decisions — terminology, constraints, Claude-facing rules |
| `.claude/docs/preproduction/systems-design.md` | Every system, its responsibility, dependencies, and tier |
| `.claude/docs/preproduction/technical-preferences.md` | Engine version, platform, performance budgets, testing requirements |
| `.claude/docs/preproduction/best-practices.md` | Project-critical patterns + Unity 6 current patterns |
| `.claude/docs/preproduction/architecture.md` | Script responsibilities, component patterns |

### Production docs (`.claude/docs/production/`)
| Path | Purpose |
|---|---|
| `.claude/docs/production/technical-design/<system>.md` | Per-system GDD — one file per system |
| `.claude/template-docs/design/technical-design/_template.md` | Per-system GDD template — copy for each system |

### Template docs (`.claude/template-docs/technical/`)
| Path | Purpose |
|---|---|
| `.claude/template-docs/technical/coding-style.md` | Required patterns and anti-patterns |
| `.claude/template-docs/technical/asset-conventions.md` | Folder layout, naming rules, import settings |

### Beta docs (`.claude/docs/beta/`)
| Path | Purpose |
|---|---|
| `.claude/template-docs/process/onboarding.md` | Setup guide — how to open, run, and test the project |
| `.claude/docs/beta/build-notes.md` | Unity version, platforms, build steps, release checklist |
| `.claude/docs/beta/known-issues.md` | Open/fixed bugs — check before implementing related systems |
| `.claude/docs/beta/changelog.md` | Milestone log |

### Agents
| Path | Purpose |
|---|---|
| `.claude/agents/gameplay-programmer.md` | Sub-agent that implements features in Unity via coplay |
| `.claude/agents/technical-director.md` | Sub-agent that reviews architecture (read-only) |

### Architecture Decision Records
| Path | Purpose |
|---|---|
| `.claude/docs/relevant-doc/adr/` | One file per significant technical decision |

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

1. `.claude/docs/preproduction/game-vision.md` — title, genre, pillars, mechanics, art/audio direction
2. `.claude/docs/preproduction/design-decisions.md` — canonical terms, spatial boundaries, locked decisions
3. `.claude/docs/preproduction/systems-design.md` — every system, tier, responsibility, and dependencies
4. `.claude/docs/preproduction/technical-preferences.md` — engine version, platform, performance budgets
5. `.claude/docs/preproduction/architecture.md` — your script table and any extra patterns

Run `/pipeline` at any time to see current progress and the next action.

### 3. Connect to Unity via coplay

Install the [coplay MCP](https://coplay.dev) and open your Unity project. Claude will use coplay tools to create GameObjects, write scripts, and test in Play Mode — no manual YAML editing needed.

### 4. Add per-system design docs

For each major system, copy `.claude/template-docs/design/technical-design/_template.md` and fill it in before implementing that system.

### 5. Start building

Ask Claude to implement features. The `gameplay-programmer` sub-agent handles implementation; the `technical-director` sub-agent handles architecture review.

---

## Prerequisites

- Unity 6 (URP)
- Claude Code CLI
- coplay MCP installed and connected to Unity
