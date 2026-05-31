using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Adds a temporary GameManager to the Game scene for in-editor testing.
/// Call CleanUp to remove it before committing.
/// </summary>
public class TestBootstrap
{
    public static void AddGameManagerToGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");

        if (GameObject.Find("GameManager") != null)
        {
            Debug.Log("GameManager already in scene.");
            return;
        }

        var go = new GameObject("GameManager");
        go.AddComponent<GameManager>();

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("Added test GameManager to Game scene.");
    }

    public static void RemoveGameManagerFromGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");

        var gm = GameObject.Find("GameManager");
        if (gm != null)
        {
            Object.DestroyImmediate(gm);
            EditorSceneManager.SaveScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            Debug.Log("Removed test GameManager from Game scene.");
        }
        else
        {
            Debug.Log("No GameManager found to remove.");
        }
    }
}
