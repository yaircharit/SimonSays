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
    public GameObject leaderboardWindow;
    public GameObject settingsWindow;

    public GameObject difficultyButtonPrefab;
    private Dictionary<string,Button> difficultyButtons = new Dictionary<string, Button>();

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
            difficultyButtons.Add(cnf,tempButt);
        }
        // Select the first difficulty by default or apply the difficulty from last game
        SelectDifficulty((selectedDifficulty == null)? difficultyButtons.Keys.First() : selectedDifficulty);

        // Add listeners to buttons (TODO: is there a better way to do this?)
        startGameButton.onClick.AddListener(StartGame);
        leaderboardButton.onClick.AddListener(()=> OpenOverlayWindow(leaderboardWindow));
        settingsButton.onClick.AddListener(() => OpenOverlayWindow(settingsWindow));
        leaderboardWindow.GetComponentInChildren<Button>().onClick.AddListener(() => CloseOverlayWindow(leaderboardWindow));
        settingsWindow.GetComponentInChildren<Button>().onClick.AddListener(() => CloseOverlayWindow(settingsWindow));

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

    public void CloseOverlayWindow(GameObject obj)
    {
        obj.SetActive(false);
        leaderboardButton.enabled = settingsButton.enabled = true;
    }


    public void OpenOverlayWindow(GameObject window)
    {
        leaderboardButton.enabled = settingsButton.enabled = false;
        window.SetActive(true);
    }

}
