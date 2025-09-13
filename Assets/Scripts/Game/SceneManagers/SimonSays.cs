using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all the visualization of in the GameScene. 
/// </summary>
public class SimonSays : MonoBehaviour
{
    public static SimonSays Instance { get; private set; }

    public GameButton buttonPrefab;
    [SerializeField]
    [Range(0f,0.5f)]
    public float radiusPrecentage = 0.3f;
    public AudioClip[] sounds;
    public static AudioClip[] Sounds => Instance.sounds;
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
    private static float GameDelay => Instance.defaultGameDelay / GameSetup.SelectedConfig.GameSpeed;
    private static GameManager gameManager;

    static SimonSays()
    {
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnTimeChanged += UpdateTime;
        gameManager = new GameManager();
        GameButton.OnButtonPress += gameManager.CheckSequence;
    }

    private void Awake()
    {
        Instance = this;

        gameManager.Init();

        overlayWindow = transform.Find("OverlayWindow").GetComponent<OnExitOverlayWindow>();
        scoreTextObject = gameObject.GetComponentsInChildren<TMP_Text>().Single(obj => obj.name == "ScoreText");
        timeTextObject = gameObject.GetComponentsInChildren<TMP_Text>().Single(obj => obj.name == "TimerText");
        repeatButton = gameObject.GetComponentsInChildren<Button>().Single(obj => obj.name == "RepeatButton");
        buttonsContainer = transform.Find("ButtonsContainer").transform;

        // Apply selected config
        repeatButton.gameObject.SetActive(!GameManager.currentGame.Challenge);        

        var rt = transform as RectTransform;

        radiusPrecentage = Mathf.Min(rt.rect.width, rt.rect.height) * radiusPrecentage * rt.localScale.x; // Adjust radius based on button count
        SpawnButtons(GameSetup.SelectedConfig.GameButtons);
    }

    private void Start()
    {
        gameManager.NextRound();
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

    private void FixedUpdate()
    {
        gameManager.UpdateGame();
    }

    public static void UpdateTime(int time)
    {
        Instance.timeTextObject.text = $"Time Left: {time} seconds";
    }

    public static void UpdateScore(float score)
    {
        Instance.scoreTextObject.text = $"Score: {score}";
    }

    private void SpawnButtons(int count)
    {
        buttons = new GameButton[count];
        float angleStep = 360.0f / count; // Angle between buttons
        Vector3 pos = new Vector3();

        for ( int i = 0; i < count; i++ )
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert angle to radians
            pos.x = Mathf.Cos(angle) * radiusPrecentage;
            pos.y = Mathf.Sin(angle) * radiusPrecentage;
            buttons[i] = Instantiate(buttonPrefab, pos, Quaternion.identity, buttonsContainer);
        }
    }

    public void EnableButtons(bool enable)
    {
        repeatButton.enabled = enable;
        Array.ForEach(buttons, (butt) => butt.enabled = enable);
    }

    public void HandleRepeatButtonClick()
    {
        if ( Instance.playSequenceCoroutine != null )
        {
            Instance.StopCoroutine(Instance.playSequenceCoroutine); // Stop previous sequence playing
        }

        Instance.playSequenceCoroutine = Instance.StartCoroutine(PlaySequence()); // Start new sequence play
    }

    public void HandleExitButtonClick()
    {
        overlayWindow.OpenWindow();
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
            yield return PlayButton(gameManager.Sequence.Last());
            EnableButtons(true);
            yield break;
        }

        // play sequance
        foreach ( var butt in gameManager.Sequence )
        {
            yield return PlayButton(butt);
        }

        EnableButtons(!overlayWindow.IsActive); // Keep disabled if overlay window is open
        playSequenceCoroutine = null; // Clear
    }

    private IEnumerator PlayButton(int index)
    {
        Instance.StartCoroutine(buttons[index].ActivateButton());
        yield return new WaitForSeconds(GameDelay);
    }
}
