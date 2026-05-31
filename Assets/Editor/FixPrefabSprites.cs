using UnityEditor;
using UnityEngine;
using System.IO;

public class FixPrefabSprites
{
    // Creates a 100x100 white PNG sprite at 100 PPU → 1x1 world unit
    // Prefab scale then controls visual size: scale(3,0.3,1) → 3×0.3 world units
    public static void Execute()
    {
        CreateSpriteAsset("Assets/Art/Sprites/pixel_white.png", false);
        CreateSpriteAsset("Assets/Art/Sprites/pixel_circle.png", true);

        RebuildPaddlePrefab();
        RebuildBallPrefab();
        RebuildBrickPrefab();

        // Update scene instances from new prefabs
        UpdateSceneInstances();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("FixPrefabSprites complete.");
    }

    // ── Sprite creation ─────────────────────────────────────────────────

    private static void CreateSpriteAsset(string path, bool circle)
    {
        if (File.Exists(path)) return;

        const int size = 100;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        float cx = size * 0.5f, cy = size * 0.5f, r = size * 0.5f - 1f;

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            if (circle)
            {
                float dx = x - cx, dy = y - cy;
                tex.SetPixel(x, y, (dx * dx + dy * dy) <= r * r ? Color.white : Color.clear);
            }
            else
                tex.SetPixel(x, y, Color.white);
        }
        tex.Apply();

        File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.ImportAsset(path);

        var imp = AssetImporter.GetAtPath(path) as TextureImporter;
        if (imp != null)
        {
            imp.textureType          = TextureImporterType.Sprite;
            imp.spritePixelsPerUnit  = 100;
            imp.filterMode           = FilterMode.Bilinear;
            imp.alphaIsTransparency  = true;
            imp.SaveAndReimport();
        }
        Debug.Log($"Created sprite: {path}");
    }

    // ── Prefab rebuild helpers ──────────────────────────────────────────

    private static Sprite WhiteSprite  => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Sprites/pixel_white.png");
    private static Sprite CircleSprite => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Sprites/pixel_circle.png");

    private static void RebuildPaddlePrefab()
    {
        const string path = "Assets/Prefabs/Paddle.prefab";
        AssetDatabase.DeleteAsset(path);

        var go = new GameObject("Paddle");
        go.tag = "Paddle";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = WhiteSprite;
        sr.color  = Color.white;
        go.transform.localScale = new Vector3(3f, 0.3f, 1f);

        var col = go.AddComponent<BoxCollider2D>();
        col.size = Vector2.one;                                  // local → world 3×0.3

        var rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType      = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        go.AddComponent<PaddleController>();

        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Rebuilt Paddle.prefab");
    }

    private static void RebuildBallPrefab()
    {
        const string path = "Assets/Prefabs/Ball.prefab";
        AssetDatabase.DeleteAsset(path);

        var go = new GameObject("Ball");

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = CircleSprite;
        sr.color  = new Color(0.4f, 0.9f, 1f, 1f);
        go.transform.localScale = Vector3.one * 0.4f;           // visual 0.4×0.4

        var mat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>("Assets/Art/Materials/BallPhysics.physicsMaterial2D");
        var col = go.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;                                       // local radius 0.5 × scale 0.4 = 0.2 world
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
        Debug.Log("Rebuilt Ball.prefab");
    }

    private static void RebuildBrickPrefab()
    {
        const string path = "Assets/Prefabs/Brick.prefab";
        AssetDatabase.DeleteAsset(path);

        var go = new GameObject("Brick");

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = WhiteSprite;
        sr.color  = Color.white;
        go.transform.localScale = new Vector3(1.6f, 0.5f, 1f);  // 1.6×0.5 world

        var col = go.AddComponent<BoxCollider2D>();
        col.size = Vector2.one;                                  // local → world 1.6×0.5

        go.AddComponent<Brick>();

        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Rebuilt Brick.prefab");
    }

    // ── Update scene instances ──────────────────────────────────────────

    private static void UpdateSceneInstances()
    {
        // Replace Paddle instance
        ReplaceInstance("Paddle", "Assets/Prefabs/Paddle.prefab", new Vector3(0f, -4.5f, 0f), go =>
        {
            var ih = GameObject.Find("InputHandler")?.GetComponent<InputHandler>();
            SetPrivateField(go.GetComponent<PaddleController>(), "_inputHandler", ih);
        });

        // Replace Ball instance
        ReplaceInstance("Ball", "Assets/Prefabs/Ball.prefab", new Vector3(0f, -4.1f, 0f), go =>
        {
            var paddle = GameObject.Find("Paddle")?.GetComponent<PaddleController>();
            SetPrivateField(go.GetComponent<BallController>(), "_paddle", paddle);
        });

        // BrickManager doesn't have a prefab — update _ball reference in case it changed
        var bm = GameObject.Find("BrickManager")?.GetComponent<BrickManager>();
        if (bm != null)
        {
            var ball = GameObject.Find("Ball")?.GetComponent<BallController>();
            SetPrivateField(bm, "_ball", ball);
        }
    }

    private static void ReplaceInstance(string goName, string prefabPath, Vector3 pos, System.Action<GameObject> wire)
    {
        var old = GameObject.Find(goName);
        if (old != null) Object.DestroyImmediate(old);

        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = pos;
        wire(instance);
    }

    private static void SetPrivateField(object target, string fieldName, object value)
    {
        if (target == null) return;
        target.GetType()
              .GetField(fieldName,
                  System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
              ?.SetValue(target, value);
    }
}
