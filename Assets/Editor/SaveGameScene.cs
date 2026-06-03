using UnityEditor;
using UnityEditor.SceneManagement;

public class SaveGameScene
{
    public static void Execute()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Game");
        if (scene.IsValid())
        {
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/Game.unity");
            UnityEngine.Debug.Log("SaveGameScene: Saved to Assets/Scenes/Game.unity");
        }
        else
        {
            UnityEngine.Debug.LogError("SaveGameScene: Game scene not found.");
        }
    }
}
