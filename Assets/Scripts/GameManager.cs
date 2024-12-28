using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    public static bool isRunning = false;
    public static int score = 0;
    public static float time = 999;

    public static List<int> sequence = new List<int>();
    public static int sequenceIndex = 0;

    readonly static System.Random rand = new System.Random();

    private void Awake()
    {
        Instance = this;
        sequence.Clear();
        sequenceIndex = 0;
        score = 0;
        time = GlobalVariables.SelectedConfig.GameTime;
        isRunning = true;
    }

    // Starts the game
    private void Start()
    {
        NextRound();
    }

    // Update time and check if the game is over
    private void FixedUpdate()
    {
        if ( isRunning )
        {
            if ( time > 0 )
            {
                time -= Time.deltaTime;
                ViewManager.Instance.UpdateTime((int)time);
            } else
            {
                EndGame(true);
            }
        }
    }

    /// <summary>
    /// Adds a new step to the sequence and plays it
    /// </summary>
    private void NextRound()
    {
        ViewManager.EnableButtons(false);
        sequence.Add(rand.Next(GlobalVariables.SelectedConfig.GameButtons));
        ViewManager.Instance.HandleRepeatButtonClick();
    }

    /// <summary>
    /// Checks if the index is the next one in the sequence
    /// </summary>
    /// <param name="index">button index</param>
    public void CheckSequence(int index)
    {
        if ( sequence[sequenceIndex++] == index )
        {
            // Correct button pressed

            if ( sequenceIndex == sequence.Count )
            {
                // All sequence pressed correctly
                
                score += GlobalVariables.SelectedConfig.PointsEachStep;
                ViewManager.Instance.UpdateScore(score);
                sequenceIndex = 0;

                NextRound();
            }
        } else
        {
            // Wrong button pressed
            EndGame(false);
        }
    }

    public void EndGame(bool gameWon)
    {
        GlobalVariables.Score = score;
        GlobalVariables.GameWon = gameWon;
        SceneManager.LoadScene("Leaderboard");
    }
   
}
