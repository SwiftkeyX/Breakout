using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AddWallRenderers
{
    public static void Execute()
    {
        string[] wallNames = { "WallLeft", "WallRight", "WallTop" };

        foreach (string wallName in wallNames)
        {
            GameObject wall = GameObject.Find("PlayAreaBounds/" + wallName);
            if (wall == null)
            {
                Debug.LogWarning("Could not find " + wallName);
                continue;
            }

            SpriteRenderer sr = wall.GetComponent<SpriteRenderer>();
            if (sr == null)
                sr = wall.AddComponent<SpriteRenderer>();

            sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.color = Color.black;

            BoxCollider2D col = wall.GetComponent<BoxCollider2D>();
            if (col != null)
                sr.size = col.size;

            EditorUtility.SetDirty(wall);
        }

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("Wall renderers added with black color.");
    }
}
