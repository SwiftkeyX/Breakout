using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RebuildPrefabs
{
    public static void Execute()
    {
        var white  = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Sprites/pixel_white.png");
        var circle = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Sprites/pixel_circle.png");
        var ballMat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>("Assets/Art/Materials/BallPhysics.physicsMaterial2D");

        if (white == null)  { Debug.LogError("pixel_white.png not found or not imported as Sprite."); return; }
        if (circle == null) { Debug.LogError("pixel_circle.png not found or not imported as Sprite."); return; }

        BuildPaddle(white);
        BuildBall(circle, ballMat);
        BuildBrick(white);

        // Refresh scene instances
        RefreshScene(white);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("RebuildPrefabs complete.");
    }

    private static void BuildPaddle(Sprite white)
    {
        const string path = "Assets/Prefabs/Paddle.prefab";
        AssetDatabase.DeleteAsset(path);
        var go = new GameObject("Paddle");
        go.tag = "Paddle";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = white;
        sr.color  = Color.white;
        go.transform.localScale = new Vector3(3f, 0.3f, 1f);

        var col = go.AddComponent<BoxCollider2D>();
        col.size = Vector2.one;

        var rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType      = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        go.AddComponent<PaddleController>();
        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Built Paddle.prefab");
    }

    private static void BuildBall(Sprite circle, PhysicsMaterial2D mat)
    {
        const string path = "Assets/Prefabs/Ball.prefab";
        AssetDatabase.DeleteAsset(path);
        var go = new GameObject("Ball");

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = circle;
        sr.color  = new Color(0.4f, 0.9f, 1f, 1f);
        go.transform.localScale = Vector3.one * 0.4f;

        var col = go.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;
        if (mat != null) col.sharedMaterial = mat;

        var rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType               = RigidbodyType2D.Dynamic;
        rb.interpolation          = RigidbodyInterpolation2D.Interpolate;
        rb.gravityScale           = 0f;
        rb.constraints            = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        go.AddComponent<BallController>();
        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Built Ball.prefab");
    }

    private static void BuildBrick(Sprite white)
    {
        const string path = "Assets/Prefabs/Brick.prefab";
        AssetDatabase.DeleteAsset(path);
        var go = new GameObject("Brick");

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = white;
        sr.color  = Color.white;
        go.transform.localScale = new Vector3(1.6f, 0.5f, 1f);

        var col = go.AddComponent<BoxCollider2D>();
        col.size = Vector2.one;

        go.AddComponent<Brick>();
        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Built Brick.prefab");
    }

    private static void RefreshScene(Sprite white)
    {
        // Open Game scene and replace instances
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");

        Replace("Paddle", "Assets/Prefabs/Paddle.prefab", new Vector3(0f, -4.5f, 0f), go =>
        {
            var ih = GameObject.Find("InputHandler")?.GetComponent<InputHandler>();
            SetField(go.GetComponent<PaddleController>(), "_inputHandler", ih);
        });

        Replace("Ball", "Assets/Prefabs/Ball.prefab", new Vector3(0f, -4.1f, 0f), go =>
        {
            var paddle = GameObject.Find("Paddle")?.GetComponent<PaddleController>();
            SetField(go.GetComponent<BallController>(), "_paddle", paddle);
        });

        var bm = GameObject.Find("BrickManager")?.GetComponent<BrickManager>();
        if (bm != null)
        {
            var ball = GameObject.Find("Ball")?.GetComponent<BallController>();
            SetField(bm, "_ball", ball);
        }

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    private static void Replace(string name, string prefabPath, Vector3 pos, System.Action<GameObject> wire)
    {
        var old = GameObject.Find(name);
        if (old != null) Object.DestroyImmediate(old);
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var inst = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        inst.transform.position = pos;
        wire(inst);
    }

    private static void SetField(object target, string field, object value)
    {
        if (target == null) return;
        target.GetType()
              .GetField(field, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
              ?.SetValue(target, value);
    }
}
