using UnityEditor;
using UnityEngine;

public class FixSpriteImport
{
    public static void Execute()
    {
        ForceSprite("Assets/Art/Sprites/pixel_white.png");
        ForceSprite("Assets/Art/Sprites/pixel_circle.png");
        AssetDatabase.Refresh();

        // Verify
        var white  = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Sprites/pixel_white.png");
        var circle = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Sprites/pixel_circle.png");
        Debug.Log($"pixel_white  sprite: {(white  != null ? "OK" : "STILL NULL")}");
        Debug.Log($"pixel_circle sprite: {(circle != null ? "OK" : "STILL NULL")}");
    }

    private static void ForceSprite(string path)
    {
        var imp = AssetImporter.GetAtPath(path) as TextureImporter;
        if (imp == null) { Debug.LogError($"No TextureImporter for {path}"); return; }

        imp.textureType         = TextureImporterType.Sprite;
        imp.spriteImportMode    = SpriteImportMode.Single;
        imp.spritePixelsPerUnit = 100;
        imp.filterMode          = FilterMode.Bilinear;
        imp.alphaIsTransparency = true;

        var settings = new TextureImporterSettings();
        imp.ReadTextureSettings(settings);
        settings.spriteMeshType = SpriteMeshType.FullRect;
        imp.SetTextureSettings(settings);

        EditorUtility.SetDirty(imp);
        imp.SaveAndReimport();
        Debug.Log($"Reimported as Sprite: {path}");
    }
}
