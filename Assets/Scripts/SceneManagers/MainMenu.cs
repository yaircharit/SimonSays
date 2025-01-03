using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnApplicationLoad()
    {
        var helperObject = new GameObject("OnApplicationLoadHelper",
            new System.Type[]{
                typeof(GameSetup), // Load config
                typeof(Leaderboard) // Load database
            }
        );

        SettingsWindow.LoadSettings();

        Destroy(helperObject.gameObject);
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
