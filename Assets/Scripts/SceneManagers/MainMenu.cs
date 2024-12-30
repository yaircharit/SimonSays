using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string configFileName = "config.json";
    public string databaseFileName = "leaderboard.db";
    public string tableName = "Leaderboard";


    private void Awake()
    {
        GlobalVariables.Init(configFileName,databaseFileName,tableName);
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
