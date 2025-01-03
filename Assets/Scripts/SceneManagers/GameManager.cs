using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains all of the game logic and behaviours.
/// </summary>
public class GameManager : MonoBehaviour
{
    private static float TimeRemaining { get; set; } = 999;

    public static PlayerScore currentGame { get; set; }
    public static List<int> Sequence { get; private set; } = new List<int>();
    public static int SequenceIndex { get; private set; } = 0;

    public static event Action<float> OnScoreChanged;
    public static event Action<int> OnTimeChanged;

    private static readonly System.Random rand = new System.Random();

    static GameManager()
    {
        GameButton.OnButtonPress += CheckSequence;
    }

    private void Awake()
    {
        Sequence.Clear();
        SequenceIndex = 0;
        currentGame = new PlayerScore();
        TimeRemaining = GameSetup.SelectedConfig.GameTime;
        if ( currentGame.Challenge )
        {
            Time.timeScale = GameSetup.SelectedConfig.GameSpeed; // Set game speed if challenge mode was selected
        }
    }

    private void Start()
    {
        NextRound();
    }

    private void FixedUpdate()
    {
        if ( TimeRemaining > 0 )
        {
            TimeRemaining -= Time.deltaTime;
            OnTimeChanged?.Invoke((int)TimeRemaining);
        } else
        {
            // Game won!
            EndGame(true);
        }
    }

    /// <summary>
    /// Adds a new step to the sequence and plays it
    /// </summary>
    private static void NextRound()
    {
        Sequence.Add(rand.Next(GameSetup.SelectedConfig.GameButtons));    // Get the next buttons of the sequence
        ViewManager.Instance.HandleRepeatButtonClick();
    }

    /// <summary>
    /// Checks if the index is the next one in the sequence
    /// </summary>
    /// <param name="index">button index</param>
    public static void CheckSequence(int index)
    {
        if ( Sequence[SequenceIndex++] == index )
        {
            // Correct button pressed

            if ( SequenceIndex == Sequence.Count )
            {
                // All sequence pressed correctly

                currentGame.Score += GameSetup.SelectedConfig.PointsEachStep;
                OnScoreChanged?.Invoke(currentGame.Score);
                SequenceIndex = 0;
                NextRound();
            }
        } else
        {
            // Wrong button pressed
            EndGame(false);
        }
    }

    /// <summary>
    /// Ends the game and moves to Leaderboard scene
    /// </summary>
    /// <param name="gameWon">true if the game was won; false if lost</param>
    public static void EndGame(bool gameWon)
    {
        currentGame.Score *= (currentGame.Challenge ? GameSetup.SelectedConfig.GameSpeed : 1);
        currentGame.gameWon = gameWon;
        Leaderboard.lastGame = currentGame;
        currentGame = null;

        Time.timeScale = 1; // back to normal
        SceneManager.LoadScene("Leaderboard");
    }
}
