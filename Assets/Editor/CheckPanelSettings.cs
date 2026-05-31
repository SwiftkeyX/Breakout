using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckPanelSettings
{
    public static void Execute()
    {
        var settings = AssetDatabase.LoadAssetAtPath<PanelSettings>("Assets/UI/GamePanelSettings.asset");
        if (settings == null) { Debug.Log("PanelSettings: NOT FOUND"); return; }
        Debug.Log($"PanelSettings: scaleMode={settings.scaleMode}, referenceResolution={settings.referenceResolution}, match={settings.match}, sortingOrder={settings.sortingOrder}, targetTexture={settings.targetTexture}");
    }
}
