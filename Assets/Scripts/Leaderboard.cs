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
    public string databaseLocalPath = "Resources/leaderboard.mdf";
    public GameObject rowPrefab;
    public TMP_Text titleTextObject;
    public GameObject rowsContainer;
    public Color hightlightColor = Color.yellow;

    private static SQLiteConnection dbConnection;
    private static List<PlayerScore> playerScores;
    private Dictionary<int, GameObject> rows;
    private PlayerScore lastGame;

    private string DatabasePath => Path.Combine(Application.dataPath, databaseLocalPath);

    void Awake()
    {
        // Initialize Database
        dbConnection ??= new SQLiteConnection(DatabasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        dbConnection.CreateTable<PlayerScore>();
        playerScores ??= dbConnection.Table<PlayerScore>().OrderByDescending(score => score.Score).ToList();

        if ( GlobalVariables.Score != -1 )
        {
            lastGame = SaveScore();
            GlobalVariables.Score = -1;
        }

        rows = new();
        DisplayScores();
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
        return SaveScore(new PlayerScore() { PlayerName = GlobalVariables.PlayerName, Score = GlobalVariables.Score });
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
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int Score { get; set; }
}
