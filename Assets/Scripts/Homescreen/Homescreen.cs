using Assets.Scripts;
using ConfigurationLoader;
using System.Collections;
using System.Collections.Generic;
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

    public GameObject difficultyButtonPrefab;
    private List<Button> difficultyButtons = new List<Button>();

    public string configFileName = "config.json";
    private string ConfigPath => $"{Application.dataPath}/Configs/{configFileName}";
    public static Dictionary<string, AppConfig> configs;
    public static AppConfig selectedConfig;
    public static string username = "";

    // Start is called before the first frame update
    void Start()
    {
        configs = ConfigLoader<AppConfig>.LoadConfig(ConfigPath);

        foreach ( var cnf in configs.Keys )
        {
            Button tempButt = Instantiate(difficultyButtonPrefab, difficultyButtonsContainer.transform).GetComponent<Button>();
            tempButt.GetComponentInChildren<TextMeshProUGUI>().text = cnf;
            tempButt.onClick.AddListener(() => SelectDifficulty(tempButt));
            difficultyButtons.Add(tempButt);
        }
        SelectDifficulty(difficultyButtons[0]);

        startGameButton.onClick.AddListener(StartGame);
        playerNameInput.text = username;
    }

    private void SelectDifficulty(Button selectedButton)
    {
        selectedButton.Select();
        string selectedDifficulty = selectedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        selectedConfig = configs[selectedDifficulty];
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
}
