using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Reflection;

public class RewireBrickManager
{
    public static void Execute()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");

        var bm = GameObject.Find("BrickManager")?.GetComponent<BrickManager>();
        if (bm == null) { Debug.LogError("BrickManager not found in scene."); return; }

        var t = typeof(BrickManager);
        var f = BindingFlags.NonPublic | BindingFlags.Instance;

        t.GetField("_brickPrefab",       f)?.SetValue(bm, AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Brick.prefab"));
        t.GetField("_dataStandard",      f)?.SetValue(bm, AssetDatabase.LoadAssetAtPath<BrickData>("Assets/ScriptableObjects/BrickData_Standard.asset"));
        t.GetField("_dataReinforced",    f)?.SetValue(bm, AssetDatabase.LoadAssetAtPath<BrickData>("Assets/ScriptableObjects/BrickData_Reinforced.asset"));
        t.GetField("_dataIndestructible",f)?.SetValue(bm, AssetDatabase.LoadAssetAtPath<BrickData>("Assets/ScriptableObjects/BrickData_Indestructible.asset"));

        var levels = new LevelData[5];
        for (int i = 1; i <= 5; i++)
            levels[i - 1] = AssetDatabase.LoadAssetAtPath<LevelData>($"Assets/ScriptableObjects/Level_0{i}.asset");
        t.GetField("_levels", f)?.SetValue(bm, levels);

        var ball = GameObject.Find("Ball")?.GetComponent<BallController>();
        t.GetField("_ball", f)?.SetValue(bm, ball);

        EditorUtility.SetDirty(bm);
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("BrickManager rewired and scene saved.");
    }
}
