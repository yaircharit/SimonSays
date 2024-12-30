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


// Can also be a generic and abstract class but theres no real need for that (Leaderboard<PlayerScore> : Table<T> : OverlayWindwo (even less neccessary) : MonoBehaviour)
public class Leaderboard : MonoBehaviour
{
    public GameObject rowPrefab;
    public TMP_Text titleTextObject;
    public GameObject rowsContainer;
    public Color hightlightColor = Color.yellow;

    private static string databaseFileName;
    private static string tableName;
    private static SqliteConnection dbConnection;
    private static List<PlayerScore> playerScores;
    private Dictionary<int, GameObject> rows;
    private PlayerScore lastGame;

    private static string DatabasePath => Path.Combine(Application.streamingAssetsPath, databaseFileName);

    void Awake()
    {
        GlobalVariables.Init();

        if ( GlobalVariables.Score != -1 )
        {
            lastGame = SaveScore();
        }

        rows = new();
        DisplayScores();
    }

    public static void Init(string dbFileName, string tableName)
    {

        if ( dbConnection == null )
        {
            Leaderboard.tableName = tableName;
            Leaderboard.databaseFileName = dbFileName;

            LoadDatabase();
            CreateTable();
            playerScores = LoadScores();
        }
    }

    public static void LoadDatabase()
    {
        if ( !File.Exists(DatabasePath) )
        {
            File.Create(DatabasePath);
        }

        dbConnection = new SqliteConnection($"Data Source={DatabasePath};Version=3;");
        dbConnection.Open();
    }

    private static void CreateTable()
    {
        string createTableQuery = @$"
                    CREATE TABLE IF NOT EXISTS {tableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        PlayerName TEXT NOT NULL,
                        Score FLOAT NOT NULL,
                        Difficulty INTEGER NOT NULL,
                        Challenge BOOLEAN NOT NULL
                    )";
        using var command = new SqliteCommand(createTableQuery, dbConnection);
        command.ExecuteNonQuery();
    }

    private static List<PlayerScore> LoadScores()
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
                        Score = reader.GetFloat(2),
                        Difficulty = reader.GetInt32(3),
                        Challenge = reader.GetBoolean(4)
                    });
                }
            }
        }
        return scores;
    }

    /// <summary>
    /// 
    /// </summary>
    private void DisplayScores()
    {
        int rank = 1;

        foreach ( var score in playerScores )
        {
            GameObject row = Instantiate(rowPrefab, rowsContainer.transform);
            var textComponents = row.GetComponentsInChildren<TextMeshProUGUI>();

            textComponents[0].text = rank++.ToString(); // Rank
            textComponents[1].text = score.PlayerName; // Player Name
            textComponents[3].text = score.Score.ToString(); // Score. Skipping index=2 cuz of Text component in Challenge Mode object
            
            var difficultyComponent = row.GetComponentInChildren<ChallengeModeToggle>();
            difficultyComponent.SetDifficulty(score.Difficulty); // Set difficulty level and color
            difficultyComponent.IsOn = score.Challenge; // Set challanage mode indication
            difficultyComponent.interactable = false;

            rows[score.Id] = row;
        }

        if ( GlobalVariables.Score != -1 ) // If the last game is over (win/lose)
        {
            titleTextObject.text = GlobalVariables.GameWon ? "Congratulations!" : "You Lost!";
            // TODO: add sounds (win / lose)
            HightlightRow(lastGame);
            GlobalVariables.Score = -1;
        }
    }

    public PlayerScore SaveScore()
    {
        return SaveScore(new PlayerScore() { 
            Id = playerScores.Count + 1,
            PlayerName = GlobalVariables.PlayerName, 
            Score = GlobalVariables.Score, 
            Difficulty = GlobalVariables.SelectedConfigIndex,
            Challenge = GlobalVariables.ChallengeMode
        });
    }

    private PlayerScore SaveScore(PlayerScore newScore)
    {
        using var command = new SqliteCommand($"INSERT INTO {tableName} VALUES ({newScore})", dbConnection);
        command.ExecuteNonQuery();
        playerScores.Add(newScore);
        playerScores = playerScores.OrderByDescending(score => score.Score).ToList();
        return newScore;
    }

    public void CloseWindow()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Highlight and scroll to a spesific PlayerScore displayed in the leaderboard table
    /// </summary>
    /// <param name="playerScore">PlayerScore to highlight</param>
    public void HightlightRow(PlayerScore playerScore)
    {
        rows[playerScore.Id].GetComponent<Image>().color = hightlightColor;
        ScrollTo(playerScore);
    }

    /// <summary>
    /// Scroll to the specified PlayerScore in the leaderboard table
    /// </summary>
    /// <param name="playerScore">PlayerScore to scroll to</param>
    public void ScrollTo(PlayerScore playerScore)
    {
        Canvas.ForceUpdateCanvases();
        RectTransform containerRectTransform = rowsContainer.GetComponent<RectTransform>();
        RectTransform rowRectTransform = rows[playerScore.Id].GetComponent<RectTransform>();

        Vector2 rowPosition = (Vector2)containerRectTransform.InverseTransformPoint(containerRectTransform.position) - (Vector2)containerRectTransform.InverseTransformPoint(rowRectTransform.position);
        float rowHeight = rowRectTransform.rect.height;

        containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, rowPosition.y - (rowHeight * 10));
    }

    public void OnApplicationQuit()
    {
        dbConnection.Close(); 
    }
}

[System.Serializable]
public class PlayerScore
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public float Score { get; set; }
    public int Difficulty { get; set; }
    public bool Challenge { get; set; }

    public override string ToString()
    {
        return $"{Id}, '{PlayerName}', {Score}, {Difficulty}, {(Challenge? 1 : 0)}";
    }
}
