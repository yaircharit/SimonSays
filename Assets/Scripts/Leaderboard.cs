using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    public string databaseLocalPath = "Resources/leaderboard.mdf";
    public GameObject rowPrefab;
    public TextMeshProUGUI titleTextObject;
    public GameObject rowsContainer;
    public Color hightlightColor = Color.yellow;

    private static SQLiteConnection dbConnection;
    private static List<PlayerScore> playerScores;
    private static Dictionary<int, GameObject> rows;

    private static string WindowTitle
    {
        get { return Instance.titleTextObject.GetComponent<TextMeshProUGUI>().text; }
        set { Instance.titleTextObject.GetComponent<TextMeshProUGUI>().text = value; }
    }

    private string DatabasePath => Path.Combine(Application.dataPath, databaseLocalPath);

    void Awake()
    {
        gameObject.SetActive(false);
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad(gameObject.transform.parent);

            rows = new();
            gameObject.GetComponentsInChildren<Button>()
            .Single((child) => child.name == "ExitButton")
            .onClick.AddListener(CloseWindow);

            InitializeDatabase();
            LoadScores();
        } else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDatabase()
    {
        dbConnection = new SQLiteConnection(DatabasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        dbConnection.CreateTable<PlayerScore>();
    }

    private void LoadScores()
    {
        playerScores = dbConnection.Table<PlayerScore>().OrderByDescending(score => score.Score).ToList();
        Debug.Log(playerScores.Count);
    }

    private void DisplayScores()
    {
        int rank = 1;
        foreach ( var score in playerScores )
        {
            GameObject row = Instantiate(rowPrefab, rowsContainer.transform);
            var textComponents = row.GetComponentsInChildren<TextMeshProUGUI>();

            textComponents[0].text = rank.ToString(); // Rank
            textComponents[1].text = score.PlayerName; // Player Name
            textComponents[2].text = score.Score.ToString(); // Score

            rank++;

            Debug.Log(score);
            rows.Add(score.Id, row);
        }
    }

    public PlayerScore SaveScore(int score = -1)
    {
        return SaveScore(new PlayerScore() { PlayerName = GlobalVariables.PlayerName, Score = score });
    }
    public PlayerScore SaveScore(PlayerScore newScore)
    {
        dbConnection.Insert(newScore);
        playerScores.Add(newScore);
        playerScores = playerScores.OrderByDescending(score => score.Score).ToList();
        return newScore;
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
        if ( SceneManager.GetActiveScene().name == "SimonSays" )
        {
            SceneManager.LoadScene("Homescreen");
        }

        // Remove all rows - should change by the next time the window is opened
        Array.ForEach(rows.Values.ToArray(), row => Destroy(row));
        rows.Clear();
    }

    public void OpenWindow(string title = "Leaderboard", int toHighlight = -1)
    {
        WindowTitle = title;
        gameObject.SetActive(true);
        DisplayScores();
        if ( toHighlight >= 0 )
            HightlightPlayerScore(toHighlight);
    }

    public void HightlightPlayerScore(int playerScoreId)
    {
        rows[playerScoreId].GetComponent<Image>().color = hightlightColor;
    }

}

[System.Serializable]
public class PlayerScore
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int Score { get; set; }
}
