using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles all the visualization of in the GameScene. 
/// </summary>
public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

    public GameObject buttonPrefab;
    public float buttonsRadius = 2.7f;
    public AudioClip[] sounds;
    public Color[] buttonColors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };
    public static Color[] ButtonColors => Instance.buttonColors;
    public float defaultGameDelay = 0.8f;

    private Button repeatButton;
    private OnExitOverlayWindow overlayWindow;

    private TMP_Text scoreTextObject;
    private TMP_Text timeTextObject;

    private Transform buttonsContainer;
    private GameButton[] buttons;
    private Coroutine playSequenceCoroutine;
    private float GameDelay => defaultGameDelay / GameSetup.SelectedConfig.GameSpeed;

    static ViewManager()
    {
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnTimeChanged += UpdateTime;
        GameManager.OnGameEnded += HandleGameEnded;
    }

    private void Awake()
    {
        Instance = this;

        overlayWindow = transform.Find("OverlayWindow").GetComponent<OnExitOverlayWindow>();
        scoreTextObject = gameObject.GetComponentsInChildren<TMP_Text>().Single(obj => obj.name == "ScoreText");
        timeTextObject = gameObject.GetComponentsInChildren<TMP_Text>().Single(obj => obj.name == "TimerText");
        repeatButton = gameObject.GetComponentsInChildren<Button>().Single(obj => obj.name == "RepeatButton");
        buttonsContainer = transform.Find("ButtonsContainer").transform;

        // Apply selected config
        repeatButton.gameObject.SetActive(!GameManager.currentGame.Challenge);        

        SpawnButtons(GameSetup.SelectedConfig.GameButtons);
    }

    public static void UpdateTime(int time)
    {
        Instance.timeTextObject.text = $"Time Left: {time} seconds";
    }

    public static void UpdateScore(float score)
    {
        Instance.scoreTextObject.text = $"Score: {score}";
    }

    private static void HandleGameEnded(bool gameWon)
    {
        SceneManager.LoadScene("Leaderboard");
    }

    private void SpawnButtons(int count)
    {
        buttons = new GameButton[count];
        float angleStep = 360.0f / count; // Angle between buttons
        Vector3 pos = new Vector3();

        for ( int i = 0; i < count; i++ )
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert angle to radians
            pos.x = Mathf.Cos(angle) * buttonsRadius;
            pos.y = Mathf.Sin(angle) * buttonsRadius;
            buttons[i] = Instantiate(buttonPrefab, pos, Quaternion.identity, buttonsContainer).GetComponent<GameButton>();
        }
    }

    public static void EnableButtons(bool enable)
    {
        Instance.repeatButton.enabled = enable;
        Array.ForEach(Instance.buttons, (butt) => butt.enabled = enable);
    }

    public void HandleRepeatButtonClick()
    {
        if ( playSequenceCoroutine != null )
        {
            StopCoroutine(playSequenceCoroutine); // Stop previous sequence playing
        }

        playSequenceCoroutine = StartCoroutine(PlaySequence()); // Start new sequence play
    }

    public void HandleExitButtonClick()
    {
        overlayWindow.OpenWindow();
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            HandleExitButtonClick();
        } else if ( Input.GetKeyDown(KeyCode.Space) && !GameManager.currentGame.Challenge )
        {
            HandleRepeatButtonClick();
        }
    }

    /// <summary>
    /// Plays the sequence of buttons. Disables buttons while playing
    /// </summary>
    public IEnumerator PlaySequence()
    {
        EnableButtons(false);
        yield return new WaitForSeconds(GameDelay * 1.5f); // Little pause before next round

        if ( !GameSetup.SelectedConfig.RepeatMode )
        {
            // Play only the last button in sequence
            yield return PlayButton(GameManager.Sequence.Last());
            EnableButtons(true);
            yield break;
        }

        // play sequance
        foreach ( var butt in GameManager.Sequence )
        {
            yield return PlayButton(butt);
        }

        EnableButtons(!overlayWindow.IsActive); // Keep disabled if overlay window is open
        playSequenceCoroutine = null; // Clear
    }

    private IEnumerator PlayButton(int index)
    {
        StartCoroutine(buttons[index].ActivateButton());
        yield return new WaitForSeconds(GameDelay);
    }
}
