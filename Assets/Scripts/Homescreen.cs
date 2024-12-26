using Assets.Scripts;
using ConfigurationLoader;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    private static Dictionary<string, Button> buttons = new Dictionary<string, Button>();
    
    private static Color selectedColor;
    private static Color defaultColor;

    public string configFileName = "config.json";
    private string ConfigPath => $"{Application.dataPath}/Configs/{configFileName}";
    public static Dictionary<string, AppConfig> configs;

    public static string selected = null;
    public static string username = "";
    public static AppConfig SelectedConfig => configs[selected];

    // Start is called before the first frame update
    void Awake()
    {
        // Load congifurations
        configs = ConfigLoader<AppConfig>.LoadConfig(ConfigPath);

        // Add and initialize difficulty buttons per config
        foreach ( var cnf in configs.Keys )
        {
            Button tempButt = Instantiate(difficultyButtonPrefab, difficultyButtonsContainer.transform).GetComponent<Button>(); 
            tempButt.GetComponentInChildren<TextMeshProUGUI>().text = cnf; 
            tempButt.onClick.AddListener(() => SelectDifficulty(cnf));
            buttons.Add(cnf, tempButt);
        }
        // Save default and selected colors
        defaultColor = buttons.Values.First().GetComponent<Button>().colors.normalColor;
        selectedColor = buttons.Values.First().GetComponent<Button>().colors.selectedColor;

        // Select the first difficulty by default or apply the difficulty from last game
        SelectDifficulty((selected == null) ? buttons.Keys.First() : selected);

        // Add listeners to buttons (TODO: is there a better way to do this?)
        startGameButton.onClick.AddListener(StartGame);
        leaderboardButton.onClick.AddListener(() => leaderboardWindow.SetActive(true));
        settingsButton.onClick.AddListener(() => settingsWindow.SetActive(true));
        leaderboardWindow.GetComponentsInChildren<Button>()
            .Single((child) => child.name == "ExitButton")
            .onClick.AddListener(() => leaderboardWindow.SetActive(false));
        settingsWindow.GetComponentsInChildren<Button>()
            .Single((child) => child.name == "ExitButton")
            .onClick.AddListener(() => settingsWindow.SetActive(false));
        exitGameButton.onClick.AddListener(() => Quit());

        playerNameInput.text = username; // Set the player name from last game or empty string
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
        buttons[difficulty].Select();
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
        var colors = buttons[selected].GetComponent<Button>().colors;
        colors.normalColor = color;
        buttons[selected].GetComponent<Button>().colors = colors;
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

        username = playerName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SimonSays");
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
