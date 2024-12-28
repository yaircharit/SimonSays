using Assets.Scripts;
using ConfigurationLoader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string configLocalPath = "Resources/config.json";
    private string configPath => Path.Combine(Application.dataPath, configLocalPath);

    private void Awake()
    {
        // Load config if not already loaded
        GlobalVariables.Configs ??= ConfigLoader<AppConfig>.LoadConfig(configPath); // do it here so there won't be any delays when moving to GameSetup
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameSetup");
    }

    public void OnSettingsButtonClick()
    {
        //TODO: use overlay window?
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
