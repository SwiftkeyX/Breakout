# Known Issues

> Living bug list. Update status as issues are resolved. Claude checks this before implementing related systems to avoid re-introducing fixed bugs.

## Open

| # | Description | Steps to Reproduce | Area | Severity |
|---|---|---|---|---|
| — | *(no issues yet)* | | | |

## Fixed

| # | Description | Fixed in |
|---|---|---|
| 1 | Ball falls below boundary without life loss; keeps falling indefinitely | Fixed 2026-06-02 — DeathZone BoxCollider2D height increased from 0.2 → 2.0 units |
| 2 | Ball freezes just below the play area on the final life; game over screen never loads and game is stuck | Fixed 2026-06-02 — Removed GameState check from `RespawnAfterDelay`; `GoToWaiting()` now called unconditionally (coroutine stops naturally when ball is destroyed on scene unload) |

## Won't Fix

| # | Description | Reason |
|---|---|---|
| — | *(none yet)* | |
