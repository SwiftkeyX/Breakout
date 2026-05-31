using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private UIDocument _doc;

    void Awake() => _doc = GetComponent<UIDocument>();

    void Start()
    {
        if (_doc == null) { Debug.LogWarning("UIManager: UIDocument not found."); return; }
        var root = _doc.rootVisualElement;
        switch (SceneManager.GetActiveScene().name)
        {
            case SceneLoader.MAIN_MENU: BindMainMenu(root); break;
            case SceneLoader.GAME:      BindHUD(root);      break;
            case SceneLoader.GAME_OVER: BindGameOver(root); break;
        }
    }

    private void BindMainMenu(VisualElement root)
    {
        root.Q<Button>("start-button").clicked += () => GameManager.Instance?.StartGame();
    }

    private void BindHUD(VisualElement root)
    {
        UpdateHUD(root);
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged += _ =>
            {
                UpdateHUD(root);
                PunchScale(root.Q<Label>("score-label"));
            };
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged += _ =>
            {
                UpdateHUD(root);
                FlashRed(root.Q<Label>("lives-label"));
            };
            GameManager.Instance.OnGameStateChanged += _ => UpdateHUD(root);
        }
    }

    private void BindGameOver(VisualElement root)
    {
        int finalScore = ScoreManager.Instance?.Score ?? 0;
        root.Q<Label>("score-label").text = $"SCORE  {finalScore:N0}";

        var gm = GameManager.Instance;
        if (gm == null) return;

        if (gm.CurrentLevelIndex >= 5)
            root.Q<Label>("result-label").text = "YOU WIN";

        root.Q<Button>("restart-button").clicked += () => gm.RestartGame();
        root.Q<Button>("menu-button").clicked    += () => gm.ReturnToMainMenu();
    }

    private void UpdateHUD(VisualElement root)
    {
        int score = ScoreManager.Instance?.Score ?? 0;
        int lives = GameManager.Instance?.Lives ?? 0;
        int level = (GameManager.Instance?.CurrentLevelIndex ?? 0) + 1;
        root.Q<Label>("score-label").text = $"SCORE  {score:N0}";
        root.Q<Label>("level-label").text = $"LEVEL  {level}";
        root.Q<Label>("lives-label").text = $"LIVES  {lives}";
    }

    private static void PunchScale(Label label)
    {
        if (label == null) return;
        const int STEPS = 12;
        int step = 0;
        label.schedule.Execute(() =>
        {
            step++;
            float t = step <= STEPS / 2
                ? (float)step / (STEPS / 2)
                : 1f - (float)(step - STEPS / 2) / (STEPS / 2);
            float s = 1f + 0.18f * t;
            label.style.scale = new StyleScale(new Scale(new Vector3(s, s, 1f)));
            if (step >= STEPS) label.style.scale = StyleKeyword.Null;
        }).Every(12).Until(() => step >= STEPS);
    }

    private static void FlashRed(Label label)
    {
        if (label == null) return;
        const int STEPS = 20;
        int step = 0;
        label.schedule.Execute(() =>
        {
            step++;
            float t = 1f - (float)step / STEPS;
            label.style.color = new StyleColor(Color.Lerp(Color.white, Color.red, t));
            if (step >= STEPS) label.style.color = StyleKeyword.Null;
        }).Every(15).Until(() => step >= STEPS);
    }
}
