using Assets.Scripts;
using ConfigurationLoader;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Homescreen : MonoBehaviour
{
    public GameObject difficultyButtonsContainer;
    public TMP_InputField playerNameInput;
    public Button startGameButton;

    public Button leaderboardButton;
    public Button settingsButton;
    public Button exitGameButton;
    public GameObject leaderboardWindow;
    public GameObject settingsWindow;

    public GameObject difficultyButtonPrefab;

    private static Color selectedColor;
    private static Color defaultColor;

    public string configLocalPath = "Resources/config.json";
    private string ConfigPath => Path.Combine(Application.dataPath, configLocalPath);
    public static Dictionary<string, AppConfig> configs;

    public static string selected = null;
    public static AppConfig SelectedConfig => configs[selected];

    // Start is called before the first frame update
    void Awake()
    {
        // Load congifurations
        configs ??= ConfigLoader<AppConfig>.LoadConfig(ConfigPath);

        // Add and initialize difficulty buttons per config
        foreach ( var cnf in configs.Keys )
        {
            Button tempButt = Instantiate(difficultyButtonPrefab, difficultyButtonsContainer.transform).GetComponent<Button>();
            tempButt.GetComponentInChildren<TextMeshProUGUI>().text = cnf;
            tempButt.onClick.AddListener(() => SelectDifficulty(cnf));

            configs[cnf].buttonRef = tempButt;
        }
        // Save default and selected colors
        // TODO: read from style sheet/config
        defaultColor = configs.Values.First().buttonRef.GetComponent<Button>().colors.normalColor;
        selectedColor = configs.Values.First().buttonRef.GetComponent<Button>().colors.selectedColor;

        // Apply player's name and difficulty from last game
        SelectDifficulty(selected ?? configs.Keys.First());
        playerNameInput.text = GlobalVariables.PlayerName;

        // Add listeners to buttons (TODO: is there a better way to do this?)
        startGameButton.onClick.AddListener(StartGame);
        leaderboardButton.onClick.AddListener(() => Leaderboard.Instance.OpenWindow());
        settingsButton.onClick.AddListener(() => settingsWindow.SetActive(true));
        settingsWindow.GetComponentsInChildren<Button>()
            .Single((child) => child.name == "ExitButton")
            .onClick.AddListener(() => settingsWindow.SetActive(false));
        exitGameButton.onClick.AddListener(() => Quit());
    }

    /// <summary>
    /// Selects a difficulty button and highlights it
    /// </summary>
    /// <param name="difficulty">Config/Difficulty name</param>
    private void SelectDifficulty(string difficulty)
    {
        if ( selected != null ) //TODO: beautify this
        {
            SetSelectedButtonNormalColor(defaultColor); // Reset the color of the previously selected button
        }
        configs[difficulty].buttonRef.Select();
        selected = difficulty;

        SetSelectedButtonNormalColor(selectedColor); // Highlight the selected button 
    }



    /// <summary>
    /// Sets the normal color of the selected button
    /// </summary>
    /// <param name="color">New color</param>
    private void SetSelectedButtonNormalColor(Color color)
    {
        //TODO: beautify this
        var colors = configs[selected].buttonRef.GetComponent<Button>().colors;
        colors.normalColor = color;
        configs[selected].buttonRef.GetComponent<Button>().colors = colors;
    }

    /// <summary>
    /// Verifies the player name and starts the game
    /// </summary>
    private void StartGame()
    {
        string playerName = playerNameInput.text.Trim();
        if ( playerName == "" )
        {
            // Highlight playerNameInput with a red border
            playerNameInput.GetComponent<Image>().color = Color.red;
            playerNameInput.Select();
            return;
        }

        GlobalVariables.selectedConfig = SelectedConfig;
        GlobalVariables.PlayerName = playerName;
        SceneManager.LoadScene("SimonSays");
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
