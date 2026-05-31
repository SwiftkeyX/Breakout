using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using System.Reflection;

public class SetupTier3
{
    public static void Execute()
    {
        CreatePanelSettings();
        CreatePowerUpPrefab();
        SetupMainMenuScene();
        SetupGameScene();
        SetupGameOverScene();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("SetupTier3 complete.");
    }

    // ── Panel Settings ───────────────────────────────────────────────────

    private static void CreatePanelSettings()
    {
        const string path = "Assets/UI/GamePanelSettings.asset";
        if (AssetDatabase.LoadAssetAtPath<PanelSettings>(path) != null) return;
        var ps = ScriptableObject.CreateInstance<PanelSettings>();
        ps.scaleMode = PanelScaleMode.ScaleWithScreenSize;
        ps.referenceResolution = new Vector2Int(1920, 1080);
        AssetDatabase.CreateAsset(ps, path);
        Debug.Log("Created GamePanelSettings.asset");
    }

    // ── PowerUp Prefab ───────────────────────────────────────────────────

    private static void CreatePowerUpPrefab()
    {
        const string path = "Assets/Prefabs/PowerUp.prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null) return;

        var go = new GameObject("PowerUp");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Sprites/pixel_circle.png");
        sr.color  = new Color(0.4f, 1f, 0.4f);
        go.transform.localScale = Vector3.one * 0.4f;

        var col = go.AddComponent<CircleCollider2D>();
        col.radius    = 0.5f;
        col.isTrigger = true;

        go.AddComponent<PowerUp>();

        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
        Debug.Log("Created PowerUp.prefab");
    }

    // ── MainMenu Scene ───────────────────────────────────────────────────

    private static void SetupMainMenuScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");

        // Add ScoreManager to GameManager GO
        var gm = GameObject.Find("GameManager");
        if (gm != null && gm.GetComponent<ScoreManager>() == null)
        {
            gm.AddComponent<ScoreManager>();
            Debug.Log("Added ScoreManager to GameManager");
        }

        // Add UIDocument + UIManager to a new UIRoot object
        SetupUIDocument("UIRoot", "Assets/UI/MainMenuUI.uxml");

        Save("MainMenu");
    }

    // ── Game Scene ───────────────────────────────────────────────────────

    private static void SetupGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");

        // HUD UIDocument
        SetupUIDocument("UIRoot", "Assets/UI/GameHUD.uxml");

        // PowerUpManager object
        SetupPowerUpManager();

        // Wire BrickManager._powerUpManager
        var bm  = GameObject.Find("BrickManager")?.GetComponent<BrickManager>();
        var pum = GameObject.Find("PowerUpManager")?.GetComponent<PowerUpManager>();
        if (bm != null && pum != null)
            SetField(bm, "_powerUpManager", pum);

        Save("Game");
    }

    private static void SetupPowerUpManager()
    {
        if (GameObject.Find("PowerUpManager") != null) return;

        var go  = new GameObject("PowerUpManager");
        var pum = go.AddComponent<PowerUpManager>();

        var paddle = GameObject.Find("Paddle")?.GetComponent<PaddleController>();
        var ball   = GameObject.Find("Ball")?.GetComponent<BallController>();
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PowerUp.prefab");

        SetField(pum, "_paddle",        paddle);
        SetField(pum, "_ball",          ball);
        SetField(pum, "_powerUpPrefab", prefab);

        Debug.Log("Created PowerUpManager");
    }

    // ── GameOver Scene ───────────────────────────────────────────────────

    private static void SetupGameOverScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/GameOver.unity");
        SetupUIDocument("UIRoot", "Assets/UI/GameOverUI.uxml");
        Save("GameOver");
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static void SetupUIDocument(string goName, string uxmlPath)
    {
        var existing = GameObject.Find(goName);
        if (existing != null) Object.DestroyImmediate(existing);

        var go   = new GameObject(goName);
        var doc  = go.AddComponent<UIDocument>();
        var ps   = AssetDatabase.LoadAssetAtPath<PanelSettings>("Assets/UI/GamePanelSettings.asset");
        var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);

        doc.panelSettings   = ps;
        doc.visualTreeAsset = uxml;

        go.AddComponent<UIManager>();
        Debug.Log($"Created UIDocument+UIManager for {uxmlPath}");
    }

    private static void Save(string sceneName)
    {
        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log($"Saved {sceneName}");
    }

    private static void SetField(object target, string name, object value)
    {
        if (target == null) return;
        target.GetType()
              .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
              ?.SetValue(target, value);
    }
}
