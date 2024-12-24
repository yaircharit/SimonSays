using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConfigurationLoader;
using System;
using Assets.Scripts;
using UnityEngine.UI;
using TMPro;
using System.Data;
using System.Threading.Tasks;

public class SimonSays : MonoBehaviour
{
    public string configFileName;
    private string ConfigPath => $"{Application.dataPath}/Configs/{configFileName}";

    public string configName;
    public GameObject buttonPrefab;
    public float buttonsRadius = 2.7f;
    public AudioClip[] sounds;
    public float defaultGameDelay = 0.8f;
    public Button nextRoundButton;
    public TextMeshProUGUI scoreTextObject;
    public TextMeshProUGUI timeTextObject;

    private AppConfig config;
    private GameButton[] gameButtons;

    private int roundNumber => sequence.Count;
    private int points = 0;
    private float time = 999;
    private bool isRunning = false;
    private float currentGameDelay;

    private List<int> sequence = new List<int>();
    private System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        config = ConfigLoader<AppConfig>.LoadConfig(ConfigPath)[configName];
        isRunning = false;


        nextRoundButton.onClick.AddListener(StartGame);
    }

    private void FixedUpdate()
    {
        if ( isRunning )
        {
            if ( time > 0 )
            {
                time -= Time.deltaTime;
                timeTextObject.text = $"Time Left: {(int)time} seconds";
            } else
            {
                Debug.Log("Game Over!");
                EndGame();
            }
        }
    }

    private async Task<bool> SpawnButtons(int count)
    {
        // TODO: when homescreen is implemented, can be moved to Start() method?
        gameButtons = new GameButton[count];
        float angleStep = 360.0f / count;
        Vector3 pos = new Vector3();

        for ( int i = 0; i < count; i++ )
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert angle to radians
            pos.x = Mathf.Cos(angle) * buttonsRadius;
            pos.y = Mathf.Sin(angle) * buttonsRadius;
            gameButtons[i] = Instantiate(buttonPrefab, pos, Quaternion.identity, transform).GetComponent<GameButton>();
            await Task.Yield(); // Allow other tasks to run
        }

        return true;
    }

    public void StartGame()
    {
        nextRoundButton.enabled = false;
        StartGameAsync();
    }

    public async Task StartGameAsync()
    {
        if ( !isRunning )
        {
           await SpawnButtons(config.GameButtons);

            time = config.GameTime;
            currentGameDelay = defaultGameDelay / config.GameSpeed;
            isRunning = true;
        }
        StartCoroutine(PlaySequance());
    }

    public void EndGame()
    {
        isRunning = false;
        timeTextObject.text = "Game Over!";
        foreach ( var gameButton in gameButtons )
        {
            Destroy(gameButton.gameObject);
        }
        gameButtons = null;
        sequence.Clear();

        //TODO: Leaderboard stuff
    }


    private IEnumerator PlaySequance()
    {
        sequence.Add(rnd.Next(config.GameButtons));

        foreach ( var gameButton in gameButtons )
        {
            gameButton.enabled = false;
        }

        // play sequance
        foreach ( var butt in sequence )
        {
            StartCoroutine(gameButtons[butt].ActivateButton());
            yield return new WaitForSeconds(currentGameDelay);
        }

        foreach ( var gameButton in gameButtons )
        {
            gameButton.enabled = false;
        }
        
        nextRoundButton.enabled = true;
    }

    private void UpdateScore()
    {
        points += config.PointsEachStep;
        scoreTextObject.text = $"Score: {points}";
    }

    private void HandlePlayerInput()
    {
        // TODO: wait for player to press buttons and validate sequance
        // TODO: allow player    to repeat the sequance (if config.RepeatMode is true)
        UpdateScore(); //TODO: Add points for each *step*, not each round
        // TODO: if player fails, end game
        // TODO: if player succeds, play next round
    }
}
