using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Data.SqlClient;

public class Leaderboard : MonoBehaviour
{
    public string databaseLocalPath = "Resources/leaderboard.mdf";
    public GameObject rowPrefab;
    public TMP_Text titleTextObject;
    public GameObject rowsContainer;
    public Color hightlightColor = Color.yellow;

    private static SqlConnection dbConnection;
    private static List<PlayerScore> playerScores;
    private Dictionary<int, GameObject> rows;
    private PlayerScore lastGame;

    private string DatabasePath => Path.Combine(Application.dataPath, databaseLocalPath);

    void Awake()
    {
        // Initialize Database
        dbConnection ??= new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={DatabasePath};Integrated Security=True");
        dbConnection.Open();
        CreateIfNotExists();

        playerScores ??= LoadScores();

        if ( GlobalVariables.Score != -1 )
        {
            lastGame = SaveScore();
            GlobalVariables.Score = -1;
        }

        rows = new();
        DisplayScores();
    }

    private void CreateIfNotExists()
    {
        // Create file if missing
        if ( !File.Exists(DatabasePath) )
        {
            File.Create(DatabasePath);
        }

        // Create Table if missing
        using ( var command = new SqlCommand("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PlayerScores' AND xtype='U') CREATE TABLE PlayerScores (Id INT PRIMARY KEY, PlayerName NVARCHAR(100), Score INT)", dbConnection) )
        {
            command.ExecuteNonQuery();
        }
    }

    private List<PlayerScore> LoadScores()
    {
        var scores = new List<PlayerScore>();
        using ( var command = new SqlCommand("SELECT * FROM PlayerScores ORDER BY Score DESC", dbConnection) )
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

            rows.Add(score.Id, row);
        }

        if ( GlobalVariables.Score != -1 ) // If the last game was won
        {
            titleTextObject.text = GlobalVariables.GameWon ? "Congratulations!" : "You Lost!";
            HightlightRow(lastGame);
        }
    }

    public PlayerScore SaveScore()
    {
        return SaveScore(new PlayerScore() { Id = playerScores.Count, PlayerName = GlobalVariables.PlayerName, Score = GlobalVariables.Score });
    }

    public PlayerScore SaveScore(PlayerScore newScore)
    {
        new SqlCommand($"INSERT INTO PlayerScores (Id, PlayerName, Score) VALUES ({newScore})", dbConnection).ExecuteNonQuery();
        playerScores.Add(newScore);
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
        return $"{Id}, {PlayerName}, {Score}";
    }
}
