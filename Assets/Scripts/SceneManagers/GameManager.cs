using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all of the game logic and behaviours.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static bool IsRunning { get; private set; } = false;
    public static float Score { get; set; } = 0;
    public static float TimeRemaining { get; private set; } = 999;
    public static bool GameWon { get; private set; } = false;
    public static bool ChallengeMode { get; set; } = false;

    public static List<int> Sequence { get; private set; } = new List<int>();
    public static int SequenceIndex { get; private set; } = 0;

    public static event Action<float> OnScoreChanged;
    public static event Action<int> OnTimeChanged;
    public static event Action<bool> OnGameEnded;

    private static readonly System.Random rand = new System.Random();

    static GameManager()
    {
        GameButton.OnButtonPress += CheckSequence;
    }

    private void Awake()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Sequence.Clear();
        SequenceIndex = 0;
        Score = 0;
        TimeRemaining = GameSetup.SelectedConfig.GameTime;
        if ( ChallengeMode )
        {
            Time.timeScale = GameSetup.SelectedConfig.GameSpeed; // Set game speed if challenge mode was selected
        }

        IsRunning = true;
    }

    private void Start()
    {
        NextRound();
    }

    private void FixedUpdate()
    {
        if ( IsRunning )
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
    }

    /// <summary>
    /// Adds a new step to the sequence and plays it
    /// </summary>
    private static void NextRound()
    {
        ViewManager.EnableButtons(false);   // Disable buttons so user can't change the sequence while playing
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

                Score += GameSetup.SelectedConfig.PointsEachStep;
                OnScoreChanged?.Invoke(Score);
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
        Score = Score * (ChallengeMode ? GameSetup.SelectedConfig.GameSpeed : 1);
        GameWon = gameWon;
        Time.timeScale = 1; // back to normal
        OnGameEnded?.Invoke(gameWon);
    }
}
