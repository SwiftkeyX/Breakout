using UnityEditor;
using UnityEngine;

public class VerifyBuildSettings
{
    public static void Execute()
    {
        var scenes = EditorBuildSettings.scenes;
        if (scenes.Length == 0)
        {
            Debug.LogWarning("Build Settings: no scenes configured!");
            return;
        }
        for (int i = 0; i < scenes.Length; i++)
            Debug.Log($"Build Settings [{i}]: {scenes[i].path} (enabled={scenes[i].enabled})");
    }
}
