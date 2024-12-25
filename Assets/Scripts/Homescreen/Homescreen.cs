using Assets.Scripts;
using ConfigurationLoader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private Dictionary<string, Button> difficultyButtons = new Dictionary<string, Button>();

    public string configFileName = "config.json";
    private string ConfigPath => $"{Application.dataPath}/Configs/{configFileName}";
    public static Dictionary<string, AppConfig> configs;

    public static string selectedDifficulty = null;
    public static string username = "";
    public static AppConfig SelectedConfig => configs[selectedDifficulty];

    // Start is called before the first frame update
    void Start()
    {
        configs = ConfigLoader<AppConfig>.LoadConfig(ConfigPath);

        foreach ( var cnf in configs.Keys )
        {
            Button tempButt = Instantiate(difficultyButtonPrefab, difficultyButtonsContainer.transform).GetComponent<Button>();
            tempButt.GetComponentInChildren<TextMeshProUGUI>().text = cnf;
            tempButt.onClick.AddListener(() => SelectDifficulty(cnf));
            difficultyButtons.Add(cnf, tempButt);
        }
        // Select the first difficulty by default or apply the difficulty from last game
        SelectDifficulty((selectedDifficulty == null) ? difficultyButtons.Keys.First() : selectedDifficulty);

        // Add listeners to buttons (TODO: is there a better way to do this?)
        startGameButton.onClick.AddListener(StartGame);
        leaderboardButton.onClick.AddListener(() => leaderboardWindow.SetActive(true));
        settingsButton.onClick.AddListener(() => settingsWindow.SetActive(true));
        leaderboardWindow.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => leaderboardWindow.SetActive(false));
        settingsWindow.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => settingsWindow.SetActive(false));
        exitGameButton.onClick.AddListener(() => Quit());

        playerNameInput.text = username;
    }

    private void SelectDifficulty(string difficulty)
    {
        difficultyButtons[difficulty].Select();
        selectedDifficulty = difficulty;
    }

    private void StartGame()
    {
        string playerName = playerNameInput.text.Trim();
        if ( playerName == "" )
        {
            // Highlight playerNameInput with a red border
            playerNameInput.GetComponent<Image>().color = Color.red;
            EventSystem.current.SetSelectedGameObject(playerNameInput.gameObject);
            return;
        }

        username = playerName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SimonSays");
    }

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
