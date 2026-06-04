# Game Vision Document

> Full creative vision — what this game is. Genre, pillars, mechanics, art/audio direction. Fill every section before development begins. Claude reads this to stay aligned with your vision throughout the project.

## Overview

| Field | Value |
|---|---|
| **Title** | Breakout 2 |
| **Genre** | Brick-breaker (Arkanoid-style) |
| **Platform** | PC — Windows |
| **Target audience** | Casual / arcade |
| **Estimated scope** | Vertical slice — 3–5 levels + main menu |

## Design Pillars

Three qualities that every decision must serve. If a feature doesn't serve at least one pillar, cut it.

1. **Chaotic** — Multi-ball mayhem and explosive power-ups dominate every session; the screen should feel gloriously out of control
2. **Explosive** — Brick destruction is visceral and over-the-top; every hit should feel satisfying and loud
3. **Fun** — Every run is short, snappy, and immediately replayable; the player should want one more attempt the moment they lose

## Core Loop

Describe the moment-to-moment and session loop:

```
Move Paddle → Deflect Ball → Destroy Bricks → Collect PowerUps → Clear Level → Next Level → repeat
```

*(Ball falls below paddle → lose a Life → respawn Ball → continue until 0 Lives remain)*

## Mechanics

### Player

| Mechanic | Description |
|---|---|
| Movement | Mouse controls Paddle horizontally along the bottom of the play area |
| Primary action | None — the Paddle is purely reactive (no fire, no dash) |
| Secondary action | None at base; power-ups may temporarily add abilities |
| Resource | Lives — player starts with 3; loses 1 each time the Ball exits the bottom boundary |

### Enemies / Obstacles

| Type | Behavior |
|---|---|
| Standard Brick | 1 hit point; destroyed on Ball contact; may drop a PowerUp |
| Reinforced Brick | 2+ hit points; changes color/opacity each hit to signal remaining HP |
| Indestructible Brick | Cannot be destroyed; acts as a permanent wall/barrier in the layout |

### Progression

Each level increases difficulty: denser Brick layouts, higher proportion of Reinforced Bricks, and a faster baseline Ball speed. Later levels introduce Indestructible Bricks to create routing puzzles.

## Win / Lose Conditions

| Condition | Trigger |
|---|---|
| **Win (level)** | All destructible Bricks cleared |
| **Win (game)** | All 3–5 levels completed |
| **Lose** | All 3 Lives lost (Ball exits bottom boundary 3 times) |

## Levels / Scenes

| Scene | Purpose |
|---|---|
| `MainMenu` | Title screen — Start / Quit |
| `Game` | Core gameplay — loads level layouts 1–5 sequentially |
| `GameOver` | End screen — shows final score, Play Again / Main Menu options |

## Art Direction

| Aspect | Direction |
|---|---|
| **Style** | Minimalist geometric — shapes only, no textures or sprites |
| **Palette** | Muted monochrome base (dark background, white/grey elements) with a single accent color (e.g. soft cyan) for the Ball and active PowerUps |
| **Camera** | Orthographic 2D, fixed — no scroll or zoom |
| **Resolution / aspect** | 1920×1080, 16:9 locked |

## Audio Direction

| Aspect | Direction |
|---|---|
| **Music style** | Ambient / atmospheric — slow evolving pads, no drums, low-key tension that escalates subtly as the level progresses |
| **SFX style** | Minimal and clean — short, satisfying clicks and tones rather than arcade blasts |
| **Key audio moments** | Brick break (soft pop/crack), PowerUp pickup (ascending chime), Ball lost (low thud), Level clear (gentle resolution chord) |

## Out of Scope

List features explicitly excluded to prevent scope creep:

- Multiplayer
- Procedural level generation
- Gamepad / controller support
- Mobile / touch support
- Online leaderboards
- Unlockable cosmetics
