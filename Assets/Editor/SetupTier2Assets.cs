using UnityEditor;
using UnityEngine;
using System.IO;

public class SetupTier2Assets
{
    public static void Execute()
    {
        AddTag("Paddle");
        AddTag("DeathZone");

        CreatePhysicsMaterial();
        CreateBrickDataAssets();
        CreateLevelDataAssets();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("SetupTier2Assets complete.");
    }

    private static void AddTag(string tag)
    {
        var tagManager = new SerializedObject(AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/TagManager.asset"));
        var tagsProp = tagManager.FindProperty("tags");
        for (int i = 0; i < tagsProp.arraySize; i++)
            if (tagsProp.GetArrayElementAtIndex(i).stringValue == tag) return;
        tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
        tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tag;
        tagManager.ApplyModifiedProperties();
        Debug.Log($"Added tag: {tag}");
    }

    private static void CreatePhysicsMaterial()
    {
        string path = "Assets/Art/Materials/BallPhysics.physicsMaterial2D";
        if (AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(path) != null) return;
        var mat = new PhysicsMaterial2D("BallPhysics") { bounciness = 1f, friction = 0f };
        AssetDatabase.CreateAsset(mat, path);
        Debug.Log("Created BallPhysics.physicsMaterial2D");
    }

    private static void CreateBrickDataAssets()
    {
        CreateBrickData("BrickData_Standard",       1,  100, Color.white,                      Color.white);
        CreateBrickData("BrickData_Reinforced",     2,  250, new Color(0.8f, 0.8f, 0.8f),      new Color(0.35f, 0.35f, 0.35f));
        CreateBrickData("BrickData_Indestructible", 99,   0, new Color(0.2f, 0.2f, 0.2f),      new Color(0.2f, 0.2f, 0.2f));
    }

    private static void CreateBrickData(string name, int hp, int pts, Color full, Color damaged)
    {
        string path = $"Assets/ScriptableObjects/{name}.asset";
        if (AssetDatabase.LoadAssetAtPath<BrickData>(path) != null) return;
        var asset = ScriptableObject.CreateInstance<BrickData>();
        asset.HitPoints       = hp;
        asset.PointValue      = pts;
        asset.FullHealthColor = full;
        asset.DamagedColor    = damaged;
        AssetDatabase.CreateAsset(asset, path);
        Debug.Log($"Created {name}");
    }

    private static void CreateLevelDataAssets()
    {
        // BrickType int values: None=0, Standard=1, Reinforced=2, Indestructible=3
        // Level 1: 5 rows x 10 cols, all Standard
        CreateLevel("Level_01", 5, 10, AllOf(5, 10, BrickType.Standard), 1.0f);

        // Level 2: 6 rows x 10 cols, top 2 rows Reinforced, rest Standard
        CreateLevel("Level_02", 6, 10, Level2Grid(), 1.1f);

        // Level 3: 6 rows x 10 cols, alternating with Indestructible borders
        CreateLevel("Level_03", 6, 10, Level3Grid(), 1.2f);

        // Level 4: 7 rows x 10 cols, denser Reinforced
        CreateLevel("Level_04", 7, 10, Level4Grid(), 1.3f);

        // Level 5: 7 rows x 10 cols, heavy Reinforced + Indestructible pillars
        CreateLevel("Level_05", 7, 10, Level5Grid(), 1.4f);
    }

    private static BrickType[] AllOf(int rows, int cols, BrickType type)
    {
        var grid = new BrickType[rows * cols];
        for (int i = 0; i < grid.Length; i++) grid[i] = type;
        return grid;
    }

    private static BrickType[] Level2Grid()
    {
        int rows = 6, cols = 10;
        var g = new BrickType[rows * cols];
        for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
            g[r * cols + c] = r < 2 ? BrickType.Reinforced : BrickType.Standard;
        return g;
    }

    private static BrickType[] Level3Grid()
    {
        int rows = 6, cols = 10;
        var g = new BrickType[rows * cols];
        for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
        {
            // Indestructible on outer edges of top row
            if (r == 0 && (c == 0 || c == cols - 1)) { g[r * cols + c] = BrickType.Indestructible; continue; }
            // Alternating Reinforced/Standard by column
            g[r * cols + c] = (c % 2 == 0) ? BrickType.Reinforced : BrickType.Standard;
        }
        return g;
    }

    private static BrickType[] Level4Grid()
    {
        int rows = 7, cols = 10;
        var g = new BrickType[rows * cols];
        for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
        {
            if (r < 3) g[r * cols + c] = BrickType.Reinforced;
            else       g[r * cols + c] = BrickType.Standard;
        }
        return g;
    }

    private static BrickType[] Level5Grid()
    {
        int rows = 7, cols = 10;
        var g = new BrickType[rows * cols];
        for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols; c++)
        {
            // Indestructible pillars at cols 2 and 7
            if (c == 2 || c == 7) { g[r * cols + c] = BrickType.Indestructible; continue; }
            g[r * cols + c] = (r % 2 == 0) ? BrickType.Reinforced : BrickType.Standard;
        }
        return g;
    }

    private static void CreateLevel(string name, int rows, int cols, BrickType[] grid, float speedMult)
    {
        string path = $"Assets/ScriptableObjects/{name}.asset";
        if (AssetDatabase.LoadAssetAtPath<LevelData>(path) != null) return;
        var asset = ScriptableObject.CreateInstance<LevelData>();
        asset.Rows               = rows;
        asset.Columns            = cols;
        asset.Grid               = grid;
        asset.BallSpeedMultiplier = speedMult;
        AssetDatabase.CreateAsset(asset, path);
        Debug.Log($"Created {name}");
    }
}
