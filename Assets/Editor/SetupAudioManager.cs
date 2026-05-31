using UnityEditor;
using UnityEngine;

public class SetupAudioManager
{
    public static void Execute()
    {
        // Remove any existing AudioManager to avoid duplicates
        var existing = GameObject.Find("AudioManager");
        if (existing != null)
        {
            Object.DestroyImmediate(existing);
            Debug.Log("SetupAudioManager: removed old AudioManager");
        }

        var go = new GameObject("AudioManager");
        var am = go.AddComponent<AudioManager>();

        am.SfxHitBrick   = Load("Assets/Audio/SFX/SfxHitBrick.wav");
        am.SfxBrickBreak = Load("Assets/Audio/SFX/SfxBrickBreak.wav");
        am.SfxHitPaddle  = Load("Assets/Audio/SFX/SfxHitPaddle.wav");
        am.SfxHitWall    = Load("Assets/Audio/SFX/SfxHitWall.wav");
        am.SfxBallLost   = Load("Assets/Audio/SFX/SfxBallLost.wav");
        am.SfxLevelClear = Load("Assets/Audio/SFX/SfxLevelClear.wav");
        am.SfxPowerUp    = Load("Assets/Audio/SFX/SfxPowerUp.wav");

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("SetupAudioManager: AudioManager added with all 7 clips assigned.");
    }

    static AudioClip Load(string path)
    {
        var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        if (clip == null) Debug.LogWarning($"SetupAudioManager: clip not found at {path}");
        return clip;
    }
}
