using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FixPowerUpManager
{
    public static void Execute()
    {
        const string scenePath = "Assets/Scenes/Game.unity";
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        var pum = Object.FindFirstObjectByType<PowerUpManager>();
        if (pum == null) { Debug.LogError("FixPowerUpManager: PowerUpManager not found."); return; }

        var ballPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Ball.prefab");
        if (ballPrefab == null) { Debug.LogError("FixPowerUpManager: Ball prefab not found."); return; }

        var so = new SerializedObject(pum);
        so.FindProperty("_ballPrefab").objectReferenceValue  = ballPrefab;
        so.FindProperty("_dropChance").floatValue            = 0.35f;
        so.ApplyModifiedProperties();

        EditorSceneManager.MarkSceneDirty(scene);
        bool saved = EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log(saved
            ? "FixPowerUpManager: Scene saved to Assets/Scenes/Game.unity."
            : "FixPowerUpManager: FAILED to save scene.");
    }
}
