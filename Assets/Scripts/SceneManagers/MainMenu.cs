using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance;
    public string configFileName = "config.json";
    public string databaseFileName = "leaderboard.db";
    public string tableName = "Leaderboard";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnApplicationLoad()
    {
        instance = new GameObject("OnApplicationLoadHelper").AddComponent<MainMenu>();

        GameSetup.Init(instance.configFileName);
        Leaderboard.Init(instance.databaseFileName, instance.tableName);
        SettingsWindow.LoadSettings();
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameSetup");
    }

    public void OnSettingsButtonClick()
    {
        SceneManager.LoadScene("Settings");
    }

    public void OnLeaderboardButtonClick()
    {
        SceneManager.LoadScene("Leaderboard");
    }
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
