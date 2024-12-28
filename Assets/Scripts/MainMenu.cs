using Assets.Scripts;
using ConfigurationLoader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string databaseFileName = "leaderboard.db";
    public string tableName = "Leaderboard";


    private void Awake()
    {
        // Load config and database if not already loaded
        // do it here so there won't be any delays when moving to GameSetup
        GlobalVariables.Configs ??= ConfigLoader<AppConfig>.LoadConfig(GlobalVariables.ConfigPath); 
        Leaderboard.Init(databaseFileName,tableName);
        SettingsWindow.LoadSettings();
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
