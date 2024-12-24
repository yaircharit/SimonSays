using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConfigurationLoader;
using System;
using Assets.Scripts;

public class SimonSays : MonoBehaviour
{
    public string configFileName;
    private string ConfigPath => $"{Application.dataPath}/Configs/{configFileName}";

    public string configName;
    public GameObject buttonPrefab;

    private AppConfig config;
    private GameButton[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        config = ConfigLoader<AppConfig>.LoadConfig(ConfigPath)[configName]; // TODO: Load in homescreen and pass to this scene

        buttons = new GameButton[config.GameButtons];

        for ( int i = 0; i < config.GameButtons; i++ )
        {
            buttons[i] = Instantiate(buttonPrefab, new Vector3(i, 0, 0), Quaternion.identity, transform).GetComponent<GameButton>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
