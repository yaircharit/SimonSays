using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeaderboardRepository
{
    private static LeaderboardRepository instance = new LeaderboardRepository();
    public static LeaderboardRepository Instance => instance;

    private static string databaseFileName;
    private static string tableName;
    private static SqliteConnection dbConnection;

    private static string DatabasePath => Path.Combine(Application.streamingAssetsPath, databaseFileName);

    private LeaderboardRepository() { }

    public static LeaderboardRepository Init(string dbFileName, string tableName)
    {
        LeaderboardRepository.databaseFileName = dbFileName;
        LeaderboardRepository.tableName = tableName;
        instance.LoadDatabase();
        instance.CreateTable();
        return instance;
    }

    private void LoadDatabase()
    {
        if ( !File.Exists(DatabasePath) )
        {
            File.Create(DatabasePath);
        }

        dbConnection = new SqliteConnection($"Data Source={DatabasePath};Version=3;");
        dbConnection.Open();
    }

    private void CreateTable()
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

    public List<PlayerScore> LoadScores()
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

    public void SaveScore(PlayerScore newScore)
    {
        using var command = new SqliteCommand($"INSERT INTO {tableName} VALUES ({newScore})", dbConnection);
        command.ExecuteNonQuery();
    }

    public void CloseConnection()
    {
        dbConnection.Close();
    }
}
