using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Core.Configs;

/// <summary>
/// Allows the user to choose which difficulty to play in (from configuration file).
/// Requires the player to enter a name and allows to play in ChallengeMode (via PlayerNameOverlayWindow)
/// </summary>
public class GameSetup : MonoBehaviour
{
    public GameObject buttonPrefab;

    private PlayerNameOverlayWindow overlayWindow;
    private Transform buttonsContainer;

    #region GlobalVars
    public static AppConfig SelectedConfig { get; set; }
    public static int SelectedConfigIndex => ConfigManager<AppConfig>.Configs.IndexOf(SelectedConfig);

    // PlayerPrefs keys
    private const string PlayerNameKey = "PlayerName";
    private const string ChallengeModeKey = "ChallengeMode";

    public static string PlayerName
    {
        get => PlayerPrefs.GetString(PlayerNameKey, ""); // Keep the player's name in the PlayerPrefs, empty string if not set
        set { PlayerPrefs.SetString(PlayerNameKey, value); }
    }
    public static bool ChallengeMode
    {
        get => PlayerPrefs.GetInt(ChallengeModeKey, 0) == 1 ? true : false;
        set { PlayerPrefs.SetInt(ChallengeModeKey, value ? 1 : 0); }
    }
    #endregion GlobalVars

    private void Start()
    {
        // Safe-guard: if loading failed or returned null, abort initialization.
        if (ConfigManager<AppConfig>.Configs == null)
        {
            Debug.LogWarning("Configs failed to load or are null. Aborting GameSetup initialization.");
            return;
        }

        overlayWindow = transform.Find("OverlayWindow").GetComponent<PlayerNameOverlayWindow>();
        buttonsContainer = transform.Find("Container").Find("ButtonsContainer");

        // Initialize buttons for each config
        foreach (var cnf in ConfigManager<AppConfig>.Configs)
        {
            Button tempButt = Instantiate(buttonPrefab, buttonsContainer).GetComponent<Button>();
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
