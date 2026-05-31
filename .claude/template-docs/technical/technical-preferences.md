# Technical Preferences

<!-- Fill this out during pre-production. All agents reference this file for project-specific standards and conventions. -->
<!-- Update whenever a technical decision is locked in. -->

## Engine & Language

| Field | Value |
|---|---|
| **Engine** | Unity 6 LTS (6000.x) |
| **Language** | C# |
| **Rendering** | URP (Universal Render Pipeline) |
| **Physics** | *(Unity 2D Physics (Box2D) / Unity 3D Physics / None)* |

## Input & Platform

| Field | Value |
|---|---|
| **Target Platforms** | *(PC / Mobile / Console)* |
| **Input Methods** | *(Mouse / Keyboard / Gamepad / Touch)* |
| **Primary Input** | *(describe the primary input method and how it maps to gameplay)* |
| **Gamepad Support** | *(Yes / No)* |
| **Touch Support** | *(Yes / No)* |

**Platform Notes**

*(Describe any input-specific implementation details here.)*

## Performance Budgets

Set these after your first profiling pass. Leave as TBD until then.

| Budget | Target | Notes |
|---|---|---|
| **Target Framerate** | *(60fps / 30fps)* | |
| **Frame Budget** | *(16.6ms / 33.3ms)* | Derived from framerate |
| **Draw Calls** | TBD — set after first profiling pass | |
| **Memory Ceiling** | TBD — set after first profiling pass | |
| **GC Alloc / Frame** | Zero in steady state | Allocations cause frame spikes |

## Testing

| Field | Value |
|---|---|
| **Framework** | NUnit (Unity Test Runner) |
| **Test types** | Edit Mode (pure logic), Play Mode (scene/runtime) |
| **Minimum Coverage** | All gameplay systems, all state machine transitions, all formulas |

**Required Tests** — list specific systems that must have tests before shipping:

- *(System name — what specifically is being tested)*
- *(System name — what specifically is being tested)*

## Forbidden Patterns

<!-- Tech-stack level rules: banned Unity subsystems, external APIs, or third-party libraries. -->
<!-- NOT for code style anti-patterns (those live in coding-style.md). -->
<!-- Format: brief name — reason -->

- `Resources.Load` — use Addressables instead
- `Input.GetKey` / `Input.GetAxis` — use Input System package instead
- UGUI Canvas — use UI Toolkit instead

## Allowed Libraries / Addons

<!-- Only add when actively integrating. Do not add speculatively. -->
<!-- Format: Package name — purpose — how approved -->

- *(None configured yet — add as dependencies are approved)*

## Architecture Decisions Log

<!-- Quick reference linking to full ADR files in docs/adr/. -->
<!-- Create a new ADR file for every significant technical decision. -->
<!-- Format: [Short title](../docs/adr/adr-NNN-title.md) — one-line summary -->

- *(No ADRs yet — create docs/adr/adr-001-*.md for your first decision)*

## Agent / Specialist Routing

<!-- Defines which Claude Code agent or skill to invoke per task type. -->
<!-- Update when you add new skills or identify tasks that need specialized handling. -->

| Task Type | Agent / Skill | Notes |
|---|---|---|
| General C# scripts, scene wiring | `gameplay-programmer` | Default for most Unity work |
| Architecture review, code audit | `technical-director` | Read-only — advises, does not implement |
| Shader / material work | `gameplay-programmer` | Minimalist style — URP Sprite Lit or Sprite Unlit only |
| UI implementation | `gameplay-programmer` | Use UI Toolkit (`.uxml` + `.uss`) |
| Asset loading / Addressables | `gameplay-programmer` | |
| Security review | `/security-review` skill | |

### File Extension Routing

| File Type | Agent to Use |
|---|---|
| `.cs` game scripts | `gameplay-programmer` |
| `.shader`, `.shadergraph`, `.mat` | `gameplay-programmer` |
| `.uxml`, `.uss`, Canvas prefabs | `gameplay-programmer` |
| `.unity`, `.prefab` | `gameplay-programmer` (via coplay MCP) |
| Architecture review | `technical-director` |
