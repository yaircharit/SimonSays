using Assets.Scripts;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

    public GameObject buttonPrefab;
    public float buttonsRadius = 2.7f;
    public AudioClip[] sounds;
    public Color[] buttonColors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan }; // TODO: read from style config
    public float defaultGameDelay = 0.8f;
    public Button repeatButton;
    public TextMeshProUGUI scoreTextObject;
    public TextMeshProUGUI timeTextObject;
    public Button exitButton;
    public GameObject leaderboardWindow;

    private static AppConfig Config => Homescreen.SelectedConfig;
    private static GameButton[] buttons;
    private static float gameDelay;


    private void Awake()
    {
        Instance = this;

        // Add listeners to buttons
        exitButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Homescreen"));
        repeatButton.onClick.AddListener(() => StartCoroutine(PlaySequance()));
        leaderboardWindow.GetComponentsInChildren<Button>()
            .Single((child) => child.name == "ExitButton")
            .onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Homescreen"));

        // Apply selected config
        repeatButton.gameObject.SetActive(Config.RepeatMode);
        gameDelay = defaultGameDelay / Config.GameSpeed;
        SpawnButtons(Config.GameButtons);
    }

    /// <summary>
    /// Update the time text
    /// </summary>
    /// <param name="time">Number of seconds left</param>
    public void UpdateTime(int time)
    {
        timeTextObject.text = $"Time Left: {time} seconds";
    }

    /// <summary>
    /// Update the score text
    /// </summary>
    /// <param name="score">New score</param>
    public void UpdateScore(int score)
    {
        scoreTextObject.text = $"Score: {GameManager.score}";
    }

    /// <summary>
    /// Spawn game buttons in a circle
    /// </summary>
    /// <param name="count">number of buttons to spawn</param>
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
            buttons[i] = Instantiate(buttonPrefab, pos, Quaternion.identity, transform).GetComponent<GameButton>();
            buttons[i].enabled = false;
        }
    }

    public static void EnableButtons(bool enable)
    {
        Instance.repeatButton.enabled = enable;
        Array.ForEach(buttons, (butt) => butt.enabled = enable);
    }

    /// <summary>
    /// Plays the sequence of buttons. Disables buttons while playing
    /// </summary>
    public IEnumerator PlaySequance()
    {
        EnableButtons(false);
        yield return new WaitForSeconds(gameDelay * 1.5f);

        // play sequance
        foreach ( var butt in GameManager.sequence )
        {
            StartCoroutine(buttons[butt].ActivateButton());
            yield return new WaitForSeconds(gameDelay);
        }

        EnableButtons(true);
    }

    public void EndGame(bool gameWon)
    {
        // Open leaderboard window and highlight the current game's score
        leaderboardWindow.GetComponentsInChildren<TextMeshProUGUI>()
            .Single((child) => child.name == "WindowTitle")
            .text = $"You {((gameWon) ? "Won" : "Lost")}!";
        leaderboardWindow.SetActive(true);
    }
}
