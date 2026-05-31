using UnityEditor;
using UnityEngine;
using System.IO;

public class SetupFolders
{
    public static void Execute()
    {
        string[] folders = new[]
        {
            "Assets/Scripts",
            "Assets/Prefabs",
            "Assets/Art",
            "Assets/Art/Sprites",
            "Assets/Art/Materials",
            "Assets/Audio",
            "Assets/Audio/Music",
            "Assets/Audio/SFX",
            "Assets/ScriptableObjects",
            "Assets/UI",
        };

        foreach (var folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                string parent = Path.GetDirectoryName(folder).Replace('\\', '/');
                string child = Path.GetFileName(folder);
                AssetDatabase.CreateFolder(parent, child);
                Debug.Log($"Created folder: {folder}");
            }
        }

        // Delete blank template script
        string templatePath = "Assets/NewMonoBehaviourScript.cs";
        if (File.Exists(Path.Combine(Application.dataPath, "../" + templatePath)))
        {
            AssetDatabase.DeleteAsset(templatePath);
            Debug.Log("Deleted blank template: " + templatePath);
        }

        AssetDatabase.Refresh();
        Debug.Log("SetupFolders complete.");
    }
}
