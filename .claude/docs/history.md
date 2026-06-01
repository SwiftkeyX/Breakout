# Project History

## 2026-06-01 — Fix MissingReferenceException on brick death

**Files changed:** `Assets/Scripts/ParticlePool.cs`, `Assets/Scripts/Brick.cs`

### What broke
Every time a brick was destroyed, Unity threw `MissingReferenceException` in
`ParticlePool.Burst()`. This was a chain of four related issues.

### Fixes

1. **ParticleSystem self-destructing after each burst**
   `BuildParticleSystem` never set `stopAction`, so Unity defaulted it to
   `Destroy`. Each non-looping system destroyed itself after playing once.
   Fix: `main.stopAction = ParticleSystemStopAction.None` and
   `main.playOnAwake = false`.

2. **Pool handing out destroyed references**
   `Burst()` had no guard against stale slots.
   Fix: null-check each slot before use; rebuild the `ParticleSystem` if destroyed.

3. **ParticlePool itself destroyed on scene reload**
   `ParticlePool` had no `DontDestroyOnLoad`, so it was torn down on every
   scene transition (level complete, game over, restart).
   Fix: added the persistent singleton pattern (`DontDestroyOnLoad` + duplicate
   guard), matching `GameManager`.

4. **C# `?.` bypasses Unity's null check**
   `Brick.Die()` used `ParticlePool.Instance?.Burst(...)`. Unity documents that
   `?.` does NOT use their overloaded `== null`, so it called `Burst` on a
   destroyed `ParticlePool`.
   Fix: `var pool = ParticlePool.Instance; if (pool != null) pool.Burst(...)`.

### Key takeaway
Never use `?.` or `??` with `UnityEngine.Object` references. Use an explicit
`if (x != null)` check so Unity's overloaded `==` operator is invoked correctly.
