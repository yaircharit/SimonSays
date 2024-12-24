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
    public float buttonsRadius = 2.7f;

    private AppConfig config;
    private GameButton[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        config = ConfigLoader<AppConfig>.LoadConfig(ConfigPath)[configName]; // TODO: Load in homescreen and pass to this scene

        buttons = new GameButton[config.GameButtons];
        float angleStep = 360.0f / config.GameButtons;
        Vector3 pos = new Vector3();

        for ( int i = 0; i < config.GameButtons; i++ )
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert angle to radians
            pos.x = Mathf.Cos(angle) * buttonsRadius;
            pos.y = Mathf.Sin(angle) * buttonsRadius;
            buttons[i] = Instantiate(buttonPrefab, pos, Quaternion.identity, transform).GetComponent<GameButton>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
