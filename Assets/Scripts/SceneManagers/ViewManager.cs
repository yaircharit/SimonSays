using Assets.Scripts;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
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
    public float defaultGameDelay = 0.8f;
    public Button repeatButton;
    public GameObject overlayWindowObject;
    private OverlayWindow overlayWindow;

    public TextMeshProUGUI scoreTextObject;
    public TextMeshProUGUI timeTextObject;
    public GameObject leaderboardWindow;

    private static GameButton[] buttons;
    private Coroutine playSeqenceCoroutine;

    private void Awake()
    {
        Instance = this;

        overlayWindow = overlayWindowObject.GetComponent<OverlayWindow>();

        // Apply selected config
        repeatButton.gameObject.SetActive(!GlobalVariables.ChallengeMode);
        SpawnButtons(GlobalVariables.SelectedConfig.GameButtons);
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

    public void HandleRepeatButtonClick()
    {
        if ( playSeqenceCoroutine != null )
        {
            StopCoroutine(playSeqenceCoroutine); // Stop previous sequence playing
        }

        playSeqenceCoroutine = StartCoroutine(PlaySequance()); // Start new sequence play
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
        } else if ( Input.GetKeyDown(KeyCode.Space) && !GlobalVariables.ChallengeMode )
        {
            HandleRepeatButtonClick();
        }
    }

    /// <summary>
    /// Plays the sequence of buttons. Disables buttons while playing
    /// </summary>
    public IEnumerator PlaySequance()
    {
        EnableButtons(false);
        yield return new WaitForSeconds(defaultGameDelay * 1.5f); // Little pause before next round

        if ( !GlobalVariables.SelectedConfig.RepeatMode )
        {
            // Play only the last button in sequence
            yield return PlayButton(GameManager.sequence.Last());
            EnableButtons(true);
            yield break;
        }

        // play sequance
        foreach ( var butt in GameManager.sequence )
        {
            yield return PlayButton(butt);
        }

        EnableButtons(true);
        playSeqenceCoroutine = null; // Clear
    }

    private IEnumerator PlayButton(int index)
    {
        StartCoroutine(buttons[index].ActivateButton());
        yield return new WaitForSeconds(defaultGameDelay);
    }
}
