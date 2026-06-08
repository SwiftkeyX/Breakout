using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceSaveScene
{
    public static void Execute()
    {
        var scene = SceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(scene);
        bool saved = EditorSceneManager.SaveScene(scene);
        Debug.Log(saved ? $"Scene '{scene.name}' force-saved." : $"Scene '{scene.name}' save FAILED.");
    }
}
