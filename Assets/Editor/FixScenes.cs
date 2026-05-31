using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FixScenes
{
    public static void Execute()
    {
        // Remove stray root-level scene file created by save_scene tool
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/MainMenu.unity") != null)
        {
            AssetDatabase.DeleteAsset("Assets/MainMenu.unity");
            Debug.Log("Deleted stray Assets/MainMenu.unity");
        }

        AssetDatabase.Refresh();
    }

    public static void SaveCurrentScene()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Saved scene: " + scene.path);
    }
}
