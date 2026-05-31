using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SetupScenes
{
    public static void Execute()
    {
        // Create MainMenu scene
        CreateScene("Assets/Scenes/MainMenu.unity");

        // Create GameOver scene
        CreateScene("Assets/Scenes/GameOver.unity");

        // Add all three scenes to Build Settings in order: MainMenu(0), Game(1), GameOver(2)
        var buildScenes = new List<EditorBuildSettingsScene>
        {
            new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true),
            new EditorBuildSettingsScene("Assets/Scenes/Game.unity", true),
            new EditorBuildSettingsScene("Assets/Scenes/GameOver.unity", true),
        };
        EditorBuildSettings.scenes = buildScenes.ToArray();
        Debug.Log("Build Settings updated: MainMenu(0), Game(1), GameOver(2)");

        AssetDatabase.Refresh();
        Debug.Log("SetupScenes complete.");
    }

    private static void CreateScene(string path)
    {
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) != null)
        {
            Debug.Log($"Scene already exists, skipping: {path}");
            return;
        }
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        EditorSceneManager.SaveScene(scene, path);
        Debug.Log($"Created scene: {path}");
    }
}
