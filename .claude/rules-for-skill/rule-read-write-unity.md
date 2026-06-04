# Rule: Unity Editor Workflow

These rules apply to any skill that reads from or writes to the Unity Editor.

## Before Any Change
Read `project-snapshot-index.md` to locate the affected GameObject, script, or asset.
If any expected section (scenes, scripts, prefabs, audio, UI assets, levels, materials, particles) is absent or incomplete, call `GenerateProjectSnapshot.Execute()` via `execute_script` immediately, tell the user what was missing, and confirm the snapshot was refreshed before continuing.

## After Writing or Editing Code
Call `check_compile_errors` — fix all errors before continuing.

## Testing
Call `play_game`, observe the specific behaviour, then call `stop_game` before marking done.

## After Scene Changes
Call `save_scene`.

## After Adding or Removing GameObjects, Scripts, or Prefabs
Run `GenerateProjectSnapshot.Execute()` via `execute_script` to update the snapshot.
