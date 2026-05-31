using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateProjectSnapshot
{
    private const string OUTPUT_PATH = ".claude/docs/project-snapshot-index.md";

    public static void Execute()
    {
        Debug.Log("GenerateProjectSnapshot: scanning project...");

        var scripts     = CollectScripts();
        var scenes      = CollectScenes();
        var prefabs     = CollectPrefabs();
        var audio       = CollectAudio();
        var hierarchies = new Dictionary<string, List<string>>();

        foreach (var scenePath in scenes)
            hierarchies[scenePath] = DumpSceneHierarchy(scenePath);

        WriteMarkdown(scripts, scenes, hierarchies, prefabs, audio);
        Debug.Log($"GenerateProjectSnapshot: written to {OUTPUT_PATH}");
    }

    // ── Collectors ──────────────────────────────────────────────────────────

    static List<string> CollectScripts()
    {
        var guids  = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Scripts" });
        var result = new List<string>();
        foreach (var g in guids)
        {
            var p = AssetDatabase.GUIDToAssetPath(g);
            if (p.EndsWith(".cs")) result.Add(p);
        }
        result.Sort();
        return result;
    }

    static List<string> CollectScenes()
    {
        var guids  = AssetDatabase.FindAssets("t:SceneAsset", new[] { "Assets" });
        var result = new List<string>();
        foreach (var g in guids)
            result.Add(AssetDatabase.GUIDToAssetPath(g));
        result.Sort();
        return result;
    }

    static List<string> CollectPrefabs()
    {
        var guids  = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
        var result = new List<string>();
        foreach (var g in guids)
            result.Add(AssetDatabase.GUIDToAssetPath(g));
        result.Sort();
        return result;
    }

    static List<(string path, string subfolder)> CollectAudio()
    {
        var guids  = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Audio" });
        var result = new List<(string, string)>();
        foreach (var g in guids)
        {
            var path  = AssetDatabase.GUIDToAssetPath(g);
            var parts = path.Split('/');
            int idx   = Array.IndexOf(parts, "Audio");
            string sub = (idx >= 0 && idx + 2 < parts.Length) ? parts[idx + 1] : "root";
            result.Add((path, sub));
        }
        result.Sort((a, b) => string.Compare(a.Item1, b.Item1, StringComparison.Ordinal));
        return result;
    }

    // ── Hierarchy dump ──────────────────────────────────────────────────────

    static List<string> DumpSceneHierarchy(string scenePath)
    {
        var lines = new List<string>();

        // Check if scene is already open
        var alreadyOpen = new HashSet<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
            alreadyOpen.Add(SceneManager.GetSceneAt(i).path);

        Scene opened = default;
        bool weOpened = false;

        if (alreadyOpen.Contains(scenePath))
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);
                if (s.path == scenePath) { opened = s; break; }
            }
        }
        else
        {
            opened   = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            weOpened = true;
        }

        if (!opened.IsValid())
        {
            lines.Add("  _(could not open scene)_");
            return lines;
        }

        foreach (var root in opened.GetRootGameObjects())
            DumpGameObject(root, 0, lines);

        if (weOpened)
            EditorSceneManager.CloseScene(opened, true);

        return lines;
    }

    static void DumpGameObject(GameObject go, int depth, List<string> lines)
    {
        var indent = new string(' ', depth * 2);
        var comps  = new List<string>();
        foreach (var c in go.GetComponents<Component>())
        {
            if (c == null) continue;
            var name = c.GetType().Name;
            if (name == "Transform") continue;
            comps.Add(name);
        }
        var compStr = comps.Count > 0 ? " [" + string.Join(", ", comps) + "]" : string.Empty;
        lines.Add($"{indent}- {go.name}{compStr}");

        for (int i = 0; i < go.transform.childCount; i++)
            DumpGameObject(go.transform.GetChild(i).gameObject, depth + 1, lines);
    }

    // ── Markdown writer ─────────────────────────────────────────────────────

    static void WriteMarkdown(
        List<string> scripts,
        List<string> scenes,
        Dictionary<string, List<string>> hierarchies,
        List<string> prefabs,
        List<(string path, string subfolder)> audio)
    {
        var sb        = new StringBuilder();
        var now       = DateTime.Now;
        var gregorian = new System.Globalization.GregorianCalendar();
        var timestamp = $"{gregorian.GetYear(now):D4}-{now.Month:D2}-{now.Day:D2} {now.Hour:D2}:{now.Minute:D2}";

        sb.AppendLine("# Project Snapshot Index");
        sb.AppendLine();
        sb.AppendLine($"_Generated: {timestamp} — run `GenerateProjectSnapshot.Execute()` to refresh._");
        sb.AppendLine();
        sb.AppendLine("> Regenerate after: adding/removing GameObjects in any scene, adding/removing scripts, prefabs, or audio assets.");
        sb.AppendLine();

        // Scenes & Hierarchies
        sb.AppendLine("## Scenes & Hierarchies");
        sb.AppendLine();
        foreach (var scenePath in scenes)
        {
            // Use path as heading to avoid duplicate names (e.g. two scenes named "Game")
            sb.AppendLine($"### {scenePath}");
            sb.AppendLine();
            sb.AppendLine();
            if (hierarchies.TryGetValue(scenePath, out var lines) && lines.Count > 0)
                foreach (var line in lines) sb.AppendLine(line);
            else
                sb.AppendLine("_(empty)_");
            sb.AppendLine();
        }

        // Scripts
        sb.AppendLine("## Scripts (`Assets/Scripts/`)");
        sb.AppendLine();
        foreach (var p in scripts)
            sb.AppendLine($"- **{Path.GetFileNameWithoutExtension(p)}** — `{p}`");
        sb.AppendLine();

        // Prefabs
        sb.AppendLine("## Prefabs (`Assets/Prefabs/`)");
        sb.AppendLine();
        if (prefabs.Count > 0)
            foreach (var p in prefabs)
                sb.AppendLine($"- **{Path.GetFileNameWithoutExtension(p)}** — `{p}`");
        else
            sb.AppendLine("_(none)_");
        sb.AppendLine();

        // Audio
        sb.AppendLine("## Audio (`Assets/Audio/`)");
        sb.AppendLine();
        var groups = new SortedDictionary<string, List<string>>();
        foreach (var (path, sub) in audio)
        {
            if (!groups.ContainsKey(sub)) groups[sub] = new List<string>();
            groups[sub].Add(path);
        }
        if (groups.Count == 0)
        {
            sb.AppendLine("_(none)_");
        }
        else
        {
            foreach (var kvp in groups)
            {
                sb.AppendLine($"**{kvp.Key}/**");
                foreach (var p in kvp.Value)
                    sb.AppendLine($"- {Path.GetFileName(p)} — `{p}`");
                sb.AppendLine();
            }
        }

        var projectRoot = Directory.GetParent(Application.dataPath).FullName;
        var fullPath    = Path.Combine(projectRoot, OUTPUT_PATH.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);
    }
}
