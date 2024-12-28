using Assets.Scripts;
using ConfigurationLoader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    
    public GameObject buttonPrefab;
    public GameObject overlayWindowObject;

    private PlayerNameOverlayWindow overlayWindow;

    void Awake()
    {
        GlobalVariables.Configs ??= ConfigLoader<AppConfig>.LoadConfig(Path.Combine(Application.streamingAssetsPath, "config.json")); // ONLY FOR TESTING
        overlayWindow = overlayWindowObject.GetComponent<PlayerNameOverlayWindow>();

        // Initialize buttons for each config
        foreach ( var cnf in GlobalVariables.Configs.Keys )
        {
            Button tempButt = Instantiate(buttonPrefab, gameObject.transform).GetComponent<Button>();
            tempButt.GetComponentInChildren<TextMeshProUGUI>().text = cnf;
            tempButt.onClick.AddListener(() => OnButtonClick(GlobalVariables.Configs[cnf]));
        }
    }

    public void OnButtonClick(AppConfig selectedConfig)
    {
        GlobalVariables.SelectedConfig = selectedConfig;

        // Open player name input overlay window
        overlayWindow.OpenWindow();
    }
}
