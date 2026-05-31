using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public const string MAIN_MENU = "MainMenu";
    public const string GAME = "Game";
    public const string GAME_OVER = "GameOver";

    public static void Load(string sceneName) =>
        SceneManager.LoadScene(sceneName);

    public static AsyncOperation LoadAsync(string sceneName) =>
        SceneManager.LoadSceneAsync(sceneName);
}
