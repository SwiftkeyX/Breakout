using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class SetupTier2Scene
{
    public static void Execute()
    {
        // Open Game scene
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");

        SetupPlayAreaBounds();
        CreatePaddlePrefab();
        CreateBallPrefab();
        CreateBrickPrefab();

        PlacePaddleInScene();
        PlaceBallInScene();
        CreateBrickManagerObject();

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();
        Debug.Log("SetupTier2Scene complete.");
    }

    // ── Play Area Bounds ────────────────────────────────────────────────

    private static void SetupPlayAreaBounds()
    {
        // Remove existing if present
        var existing = GameObject.Find("PlayAreaBounds");
        if (existing != null) Object.DestroyImmediate(existing);

        var root = new GameObject("PlayAreaBounds");

        // With orthographic size=6 and 16:9 aspect: halfW ≈ 10.667
        const float hw = 10.9f;   // half-width with slight padding
        const float hh = 6.2f;   // half-height with slight padding
        const float wallThick = 0.2f;
        const float wallLen   = 13.5f; // taller than screen

        CreateWall(root, "WallLeft",   new Vector2(-hw, 0),          new Vector2(wallThick, wallLen * 2), false);
        CreateWall(root, "WallRight",  new Vector2( hw, 0),          new Vector2(wallThick, wallLen * 2), false);
        CreateWall(root, "WallTop",    new Vector2(0,   hh),         new Vector2(hw * 2, wallThick),      false);
        CreateWall(root, "DeathZone",  new Vector2(0,  -hh - 0.3f), new Vector2(hw * 2, wallThick),      true);

        // Tag DeathZone
        var dz = root.transform.Find("DeathZone").gameObject;
        dz.tag = "DeathZone";

        Debug.Log("Created PlayAreaBounds");
    }

    private static void CreateWall(GameObject parent, string name, Vector2 pos, Vector2 size, bool isTrigger)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform);
        go.transform.position = new Vector3(pos.x, pos.y, 0f);
        var col = go.AddComponent<BoxCollider2D>();
        col.size = size;
        col.isTrigger = isTrigger;
    }

    // ── Paddle Prefab ───────────────────────────────────────────────────

    private static void CreatePaddlePrefab()
    {
        string path = "Assets/Prefabs/Paddle.prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null) return;

        var go = new GameObject("Paddle");
        go.tag = "Paddle";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = GetWhiteSprite();
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
        Debug.Log("Created Paddle.prefab");
    }

    // ── Ball Prefab ─────────────────────────────────────────────────────

    private static void CreateBallPrefab()
    {
        string path = "Assets/Prefabs/Ball.prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null) return;

        var go = new GameObject("Ball");

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = GetCircleSprite();
        sr.color  = new Color(0.4f, 0.9f, 1f, 1f);
        go.transform.localScale = Vector3.one * 0.4f;

        var mat  = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>("Assets/Art/Materials/BallPhysics.physicsMaterial2D");
        var col  = go.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;
        if (mat != null) col.sharedMaterial = mat;

        var rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType              = RigidbodyType2D.Dynamic;
        rb.interpolation         = RigidbodyInterpolation2D.Interpolate;
        rb.gravityScale          = 0f;
        rb.constraints           = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        go.AddComponent<BallController>();

        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Created Ball.prefab");
    }

    // ── Brick Prefab ────────────────────────────────────────────────────

    private static void CreateBrickPrefab()
    {
        string path = "Assets/Prefabs/Brick.prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null) return;

        var go = new GameObject("Brick");

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = GetWhiteSprite();
        sr.color  = Color.white;
        go.transform.localScale = new Vector3(1.6f, 0.5f, 1f);

        go.AddComponent<BoxCollider2D>();
        go.AddComponent<Brick>();

        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Created Brick.prefab");
    }

    // ── Place instances in scene ────────────────────────────────────────

    private static void PlacePaddleInScene()
    {
        if (GameObject.Find("Paddle") != null) return;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Paddle.prefab");
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        go.transform.position = new Vector3(0f, -4.5f, 0f);

        // Wire InputHandler reference
        var ih = GameObject.Find("InputHandler")?.GetComponent<InputHandler>();
        if (ih != null)
            go.GetComponent<PaddleController>().GetType()
              .GetField("_inputHandler",
                  System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
              ?.SetValue(go.GetComponent<PaddleController>(), ih);

        Debug.Log("Placed Paddle in scene");
    }

    private static void PlaceBallInScene()
    {
        if (GameObject.Find("Ball") != null) return;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Ball.prefab");
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        go.transform.position = new Vector3(0f, -4.1f, 0f);

        // Wire PaddleController reference
        var paddle = GameObject.Find("Paddle")?.GetComponent<PaddleController>();
        if (paddle != null)
            go.GetComponent<BallController>().GetType()
              .GetField("_paddle",
                  System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
              ?.SetValue(go.GetComponent<BallController>(), paddle);

        Debug.Log("Placed Ball in scene");
    }

    private static void CreateBrickManagerObject()
    {
        if (GameObject.Find("BrickManager") != null) return;
        var go = new GameObject("BrickManager");
        var bm = go.AddComponent<BrickManager>();

        var bmType = typeof(BrickManager);
        var flags  = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

        // Assign prefab
        var brickPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Brick.prefab");
        bmType.GetField("_brickPrefab", flags)?.SetValue(bm, brickPrefab);

        // Assign BrickData
        bmType.GetField("_dataStandard",       flags)?.SetValue(bm, AssetDatabase.LoadAssetAtPath<BrickData>("Assets/ScriptableObjects/BrickData_Standard.asset"));
        bmType.GetField("_dataReinforced",     flags)?.SetValue(bm, AssetDatabase.LoadAssetAtPath<BrickData>("Assets/ScriptableObjects/BrickData_Reinforced.asset"));
        bmType.GetField("_dataIndestructible", flags)?.SetValue(bm, AssetDatabase.LoadAssetAtPath<BrickData>("Assets/ScriptableObjects/BrickData_Indestructible.asset"));

        // Assign LevelData array
        var levels = new LevelData[5];
        for (int i = 1; i <= 5; i++)
            levels[i - 1] = AssetDatabase.LoadAssetAtPath<LevelData>($"Assets/ScriptableObjects/Level_0{i}.asset");
        bmType.GetField("_levels", flags)?.SetValue(bm, levels);

        // Assign Ball reference
        var ball = GameObject.Find("Ball")?.GetComponent<BallController>();
        bmType.GetField("_ball", flags)?.SetValue(bm, ball);

        Debug.Log("Created BrickManager object");
    }

    // ── Sprite helpers ──────────────────────────────────────────────────

    private static Sprite GetWhiteSprite()
    {
        // Use Unity's built-in white square sprite
        return Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd")
            ?? AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
    }

    private static Sprite GetCircleSprite()
    {
        // Use Unity's built-in knob (circle) sprite
        return Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd")
            ?? AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
    }
}
