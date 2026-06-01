using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupAudioManager
{
    public static void Execute()
    {
        // Remove any stray standalone AudioManager GameObject (created by mistake)
        var stray = GameObject.Find("AudioManager");
        if (stray != null && stray.GetComponent<GameManager>() == null)
        {
            Object.DestroyImmediate(stray);
            Debug.Log("SetupAudioManager: removed stray AudioManager GameObject");
        }

        // Assign clips to the AudioManager component on GameManager
        var gm = GameObject.Find("GameManager");
        if (gm == null) { Debug.LogError("SetupAudioManager: GameManager not found."); return; }

        var am = gm.GetComponent<AudioManager>();
        if (am == null) { Debug.LogError("SetupAudioManager: AudioManager component not on GameManager."); return; }

        am.SfxHitBrick   = Load("Assets/Audio/SFX/SfxHitBrick.wav");
        am.SfxBrickBreak = Load("Assets/Audio/SFX/SfxBrickBreak.wav");
        am.SfxHitPaddle  = Load("Assets/Audio/SFX/SfxHitPaddle.wav");
        am.SfxHitWall    = Load("Assets/Audio/SFX/SfxHitWall.wav");
        am.SfxBallLost   = Load("Assets/Audio/SFX/SfxBallLost.wav");
        am.SfxLevelClear = Load("Assets/Audio/SFX/SfxLevelClear.wav");
        am.SfxPowerUp    = Load("Assets/Audio/SFX/SfxPowerUp.wav");

        EditorUtility.SetDirty(gm);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

        Debug.Log("SetupAudioManager: all 7 SFX clips assigned to GameManager.AudioManager.");
    }

    static AudioClip Load(string path)
    {
        var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        if (clip == null) Debug.LogWarning($"SetupAudioManager: clip not found at {path}");
        return clip;
    }
}
