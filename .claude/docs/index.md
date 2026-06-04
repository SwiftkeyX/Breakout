# Docs Index

Before starting work, check the trigger conditions below to find the right doc. Read only what applies.

## Pre-production

**`preproduction/game-vision.md`** — genre, pillars, mechanics, art/audio direction
- Consult when: making a feel/aesthetic decision, checking genre alignment, art or audio direction

**`preproduction/design-decisions.md`** — locked decisions, canonical terminology, spatial constraints
- Consult when: unsure if something is already decided, using game terms, checking play-area dimensions

**`preproduction/systems-design.md`** — every system, tier, responsibilities, dependencies
- Consult when: designing a new system, checking what a system owns, verifying tier order

**`preproduction/architecture.md`** — script responsibilities, component ownership, input conventions
- Consult when: adding a new script, deciding where logic belongs, checking which component owns a behavior

**`preproduction/best-practices.md`** — project-critical patterns, Unity 6 current patterns
- Consult when: writing any C# code — always read this before writing scripts

**`preproduction/technical-preferences.md`** — engine version, platform, performance budgets, test requirements
- Consult when: making platform/build decisions, checking performance constraints

## Production

**`production/technical-design/<system>.md`** — per-system GDD
- Consult when: implementing or modifying a specific system — read that system's file

## Beta

**`beta/known-issues.md`** — open/fixed bugs
- Consult when: implementing a system that has known bugs, or verifying a fix is already tracked

**`beta/build-notes.md`** — Unity version, target platforms, build steps, release checklist
- Consult when: making a build, checking the Unity version, or running the release checklist

**`beta/changelog.md`** — milestone-by-milestone changelog
- Consult when: logging a completed milestone or reviewing recent significant decisions

## Process

**`PIPELINE.md`** — phase tracker; production status and unchecked items
- Consult when: checking what phase the project is in or which task is next

**`../rules-for-skill/rule-what-to-do-get-block-by-previous-step.md`** — when to call /regress instead of patching symptoms
- Consult when: a skill encounters a blocking issue rooted in a prior pipeline stage

**`../rules-for-skill/rule-pipeline-progression-update.md`** — how and when to tick PIPELINE.md items
- Consult when: completing a skill step and ready to mark it done in PIPELINE.md

**`../rules-for-skill/rule-read-write-unity.md`** — Unity editor workflow rules (compile check, play/stop, save, snapshot)
- Consult when: any skill that reads from or writes to the Unity Editor

## Template Reference

**`../template-docs/technical/coding-style.md`** — required code patterns and anti-patterns
- Consult when: unsure about code formatting, naming, or structural conventions

**`../template-docs/technical/asset-conventions.md`** — folder layout, naming rules, import settings
- Consult when: adding or renaming any asset, setting import settings

## Snapshot

**`project-snapshot-index.md`** — scene hierarchies, scripts, prefabs, audio assets
- Consult when: starting any structural work — read this first to understand current scene state

## Reference

**`history.md`** — root cause analyses and fix narratives for past issues
- Consult when: debugging a recurring problem or reviewing how a past bug was resolved

**`diagrams.md`** — system architecture and communication flow diagrams (Mermaid)
- Consult when: visualizing system dependencies, communication patterns, or tier build order
