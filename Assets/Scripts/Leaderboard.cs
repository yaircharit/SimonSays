using Assets.Scripts;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public string databaseLocalPath = "leaderboard.db";
    public string tableName = "Leaderboard";
    public GameObject rowPrefab;
    public TMP_Text titleTextObject;
    public GameObject rowsContainer;
    public Color hightlightColor = Color.yellow;

    private static SqliteConnection dbConnection;
    private static List<PlayerScore> playerScores;
    private Dictionary<int, GameObject> rows;
    private PlayerScore lastGame;

    private string DatabasePath => Path.Combine(Application.streamingAssetsPath, databaseLocalPath);

    void Awake()
    {
        // Initialize Database
        if ( dbConnection == null )
        {
            dbConnection = new SqliteConnection($"Data Source={DatabasePath};Version=3;");
            dbConnection.Open();
            CreateTable();
            playerScores = LoadScores();
        }

        if ( GlobalVariables.Score != -1 )
        {
            lastGame = SaveScore();
        }

        rows = new();
        DisplayScores();
    }

    private void CreateTable()
    {
        string createTableQuery = @$"
                    CREATE TABLE IF NOT EXISTS {tableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        PlayerName TEXT NOT NULL,
                        Score INTEGER NOT NULL
                    )";
        using var command = new SqliteCommand(createTableQuery, dbConnection);
        command.ExecuteNonQuery();
    }

    private List<PlayerScore> LoadScores()
    {
        var scores = new List<PlayerScore>();
        using ( var command = new SqliteCommand($"SELECT * FROM {tableName} ORDER BY Score DESC", dbConnection) )
        {
            using ( var reader = command.ExecuteReader() )
            {
                while ( reader.Read() )
                {
                    scores.Add(new PlayerScore {
                        Id = reader.GetInt32(0),
                        PlayerName = reader.GetString(1),
                        Score = reader.GetInt32(2)
                    });
                }
            }
        }
        return scores;
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

            rows[score.Id] = row;
        }

        if ( GlobalVariables.Score != -1 ) // If the last game is over (win/lose)
        {
            titleTextObject.text = GlobalVariables.GameWon ? "Congratulations!" : "You Lost!";
            HightlightRow(lastGame);
            GlobalVariables.Score = -1;
        }
    }

    public PlayerScore SaveScore()
    {
        return SaveScore(new PlayerScore() { PlayerName = GlobalVariables.PlayerName, Score = GlobalVariables.Score });
    }

    private PlayerScore SaveScore(PlayerScore newScore)
    {
        using var command = new SqliteCommand($"INSERT INTO {tableName} (PlayerName, Score) VALUES ({newScore})", dbConnection);
        command.ExecuteNonQuery();
        playerScores.Add(newScore);
        newScore.Id = playerScores.Count;
        playerScores = playerScores.OrderByDescending(score => score.Score).ToList();
        return newScore;
    }

    public void CloseWindow()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void HightlightRow(PlayerScore playerScore)
    {
        rows[playerScore.Id].GetComponent<Image>().color = hightlightColor;
        ScrollTo(playerScore);
    }

    public void ScrollTo(PlayerScore playerScore)
    {
        Canvas.ForceUpdateCanvases();
        RectTransform containerRectTransform = rowsContainer.GetComponent<RectTransform>();
        RectTransform rowRectTransform = rows[playerScore.Id].GetComponent<RectTransform>();

        Vector2 rowPosition = (Vector2)containerRectTransform.InverseTransformPoint(containerRectTransform.position) - (Vector2)containerRectTransform.InverseTransformPoint(rowRectTransform.position);
        float rowHeight = rowRectTransform.rect.height;

        containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, rowPosition.y - (rowHeight * 10));
    }
}

[System.Serializable]
public class PlayerScore
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int Score { get; set; }

    public override string ToString()
    {
        return $"'{PlayerName}', {Score}";
    }
}
