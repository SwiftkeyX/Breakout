using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Reflection;

public class SetupJuice
{
    public static void Execute()
    {
        AddTag("Wall");
        SetupMainMenuComponents();
        SetupGameScene();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("SetupJuice complete.");
    }

    // ── Tags ─────────────────────────────────────────────────────────────

    private static void AddTag(string tag)
    {
        var tagManager = new SerializedObject(
            AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/TagManager.asset"));
        var tagsProp = tagManager.FindProperty("tags");
        for (int i = 0; i < tagsProp.arraySize; i++)
            if (tagsProp.GetArrayElementAtIndex(i).stringValue == tag) return;
        tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
        tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tag;
        tagManager.ApplyModifiedProperties();
        Debug.Log($"Added tag: {tag}");
    }

    // ── MainMenu: add AudioManager + ParticlePool to GameManager GO ──────

    private static void SetupMainMenuComponents()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");

        var gm = GameObject.Find("GameManager");
        if (gm == null) { Debug.LogError("GameManager not found in MainMenu."); return; }

        if (gm.GetComponent<AudioManager>() == null)
        {
            gm.AddComponent<AudioManager>();
            Debug.Log("Added AudioManager to GameManager");
        }
        if (gm.GetComponent<ParticlePool>() == null)
        {
            gm.AddComponent<ParticlePool>();
            Debug.Log("Added ParticlePool to GameManager");
        }

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("Saved MainMenu");
    }

    // ── Game scene: add CameraEffects to camera + tag walls ─────────────

    private static void SetupGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");

        // Add CameraEffects to Main Camera
        var cam = GameObject.Find("Main Camera");
        if (cam != null && cam.GetComponent<CameraEffects>() == null)
        {
            cam.AddComponent<CameraEffects>();
            Debug.Log("Added CameraEffects to Main Camera");
        }

        // Tag walls
        TagChild("PlayAreaBounds/WallLeft",  "Wall");
        TagChild("PlayAreaBounds/WallRight", "Wall");
        TagChild("PlayAreaBounds/WallTop",   "Wall");

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("Saved Game");
    }

    private static void TagChild(string path, string tag)
    {
        var go = GameObject.Find(path);
        if (go == null) { Debug.LogWarning($"Not found: {path}"); return; }
        go.tag = tag;
        Debug.Log($"Tagged {path} as '{tag}'");
    }
}
