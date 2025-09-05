using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    static GameObject helperObject;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnApplicationLoad()
    {
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
