using Assets.Scripts;
using ConfigurationLoader;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Allows the user to choose which difficulty to play in (from configuration file).
/// Requires the player to enter a name and allows to play in ChallengeMode (via PlayerNameOverlayWindow)
/// </summary>
public class GameSetup : MonoBehaviour
{
    
    public GameObject buttonPrefab;
    public GameObject overlayWindowObject;

    private PlayerNameOverlayWindow overlayWindow;

    private static string ConfigFileName { get; set; } = "config.json";
    private static string ConfigPath => Path.Combine(Application.streamingAssetsPath, ConfigFileName);
    public static List<AppConfig> Configs { get; set; }

    public static AppConfig SelectedConfig { get; set; }
    public static int SelectedConfigIndex => Configs.IndexOf(SelectedConfig);


    // PlayerPrefs keys
    private const string PlayerNameKey = "PlayerName";

    public static string PlayerName
    {
        get => PlayerPrefs.GetString(PlayerNameKey, ""); // Keep the player's name in the PlayerPrefs, empty string if not set
        set { PlayerPrefs.SetString(PlayerNameKey, value); }
    }

    public static void Init(string configFileName)
    {
        ConfigFileName = configFileName;
        Configs ??= ConfigLoader<AppConfig>.LoadConfig(ConfigPath);
    }

    void Awake()
    {
        overlayWindow = overlayWindowObject.GetComponent<PlayerNameOverlayWindow>();

        // Initialize buttons for each config
        foreach ( var cnf in Configs )
        {
            Button tempButt = Instantiate(buttonPrefab, gameObject.transform).GetComponent<Button>();
            tempButt.GetComponentInChildren<TextMeshProUGUI>().text = cnf.Name;
            tempButt.onClick.AddListener(() => OnButtonClick(cnf));
        }
    }

    public void OnButtonClick(AppConfig selectedConfig)
    {
        SelectedConfig = selectedConfig;

        // Open player name input overlay window
        overlayWindow.OpenWindow();
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
