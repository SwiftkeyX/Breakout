# Build Notes

> Unity version, target platforms, build steps, and release checklist.

## Project Info

| Field | Value |
|---|---|
| **Unity version** | 6000.4.9f1 |
| **Render pipeline** | URP |
| **Target platform(s)** | Windows |
| **Min OS / browser** | Windows 11 |

## Build Steps

1. Open Build Settings (`File > Build Settings`)
2. Confirm target platform matches above
3. Run `check_compile_errors` (coplay) — fix all errors before building
4. Switch platform if needed (`Switch Platform`)
5. Click `Build` — output to `Builds/<platform>/`

## Release Checklist

- [x] All compile errors resolved
- [x] Play Mode smoke test passed (main loop, win, lose)
- [x] Performance pass done (stable frame rate, no GC spikes)
- [ ] Build runs on target device / browser
- [ ] Version number bumped in Player Settings
- [ ] Changelog entry added
