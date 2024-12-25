using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static AppConfig Config => Homescreen.SelectedConfig;


    public static bool isRunning = false;
    public static int points = 0;
    public static float time = 999;
    
    public float defaultGameDelay = 1.0f;

    public static List<int> sequence = new List<int>();
    public static int sequenceIndex = 0;

    readonly static System.Random rand = new System.Random();

    private void Awake()
    {
        Instance = this;
        sequence.Clear();
        sequenceIndex = 0;
        points = 0;
        time = Config.GameTime;
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
                Debug.Log("Game Over!");
                ViewManager.Instance.EndGame(true);
            }
        }
    }

    /// <summary>
    /// Adds a new step to the sequence and plays it
    /// </summary>
    private void NextRound()
    {
        sequence.Add(rand.Next(Config.GameButtons));
        StartCoroutine(ViewManager.Instance.PlaySequance());
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

                points += Config.PointsEachStep;
                ViewManager.Instance.UpdateScore(points);
                sequenceIndex = 0;

                NextRound();
            }
        } else
        {
            // Wrong button pressed
            ViewManager.Instance.EndGame(false);
        }
    }
}
