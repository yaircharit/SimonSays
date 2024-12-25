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
    public GameObject buttonPrefab;
    public float buttonsRadius = 2.7f;
    public AudioClip[] sounds;
    public float defaultGameDelay = 0.8f;
    public Button repeatButton;
    public TextMeshProUGUI scoreTextObject;
    public TextMeshProUGUI timeTextObject;
    public Button exitButton;

    private AppConfig config => Homescreen.selectedConfig;
    private GameButton[] gameButtons;
    private bool isLoading = true;
    private bool isPlayingSequance = false;

    private int points = 0;
    private float time = 999;
    private bool isRunning = false;
    private float currentGameDelay;

    private List<int> sequence = new List<int>();
    private System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        isRunning = false;
        exitButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Homescreen"));
        
        repeatButton.onClick.AddListener(() => StartCoroutine(PlaySequance()));
        repeatButton.gameObject.SetActive(config.RepeatMode);
        

        StartCoroutine(SpawnButtons(config.GameButtons));
        StartCoroutine(StartGame());
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
    
    private IEnumerator SpawnButtons(int count)
    {
        gameButtons = new GameButton[count];
        float angleStep = 360.0f / count;
        Vector3 pos = new Vector3();

        for ( int i = 0; i < count; i++ )
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert angle to radians
            pos.x = Mathf.Cos(angle) * buttonsRadius;
            pos.y = Mathf.Sin(angle) * buttonsRadius;
            gameButtons[i] = Instantiate(buttonPrefab, pos, Quaternion.identity, transform).GetComponent<GameButton>();
            yield return new WaitUntil(() => gameButtons[i] != null);
        }

        isLoading = false;
    }

    public IEnumerator StartGame()
    {
        repeatButton.enabled = false;
        if ( !isRunning )
        {
            time = config.GameTime;
            currentGameDelay = defaultGameDelay / config.GameSpeed;
            isRunning = true;
        }
        yield return new WaitUntil(() => !isLoading);
        NextRound();
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

    private void NextRound()
    {
        repeatButton.enabled = false;
        sequence.Add(rnd.Next(config.GameButtons));
        StartCoroutine(PlaySequance());
    }

    private IEnumerator PlaySequance()
    {
        isPlayingSequance = true;
        repeatButton.enabled = false;
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

        repeatButton.enabled = true;
        isPlayingSequance = false;
    }

    private void UpdateScore()
    {
        points += config.PointsEachStep;
        scoreTextObject.text = $"Score: {points}";
    }

    private void HandlePlayerInput()
    {
       
            // TODO: wait for player to press buttons and validate sequance
            //TODO: Add points for each *step*, not each round
            // TODO: if player fails, end game
            // TODO: if player succeds, play next round

    }
}
