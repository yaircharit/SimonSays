using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeaderboardRepository
{
    private string tableName;
    private SqliteConnection dbConnection;

    public LeaderboardRepository(string dbFileName, string tableName) 
    {
        LoadDatabase(dbFileName);
        CreateTable(tableName);
    }

    private void LoadDatabase(string dbFileName)
    {
        string DatabasePath = Path.Combine(Application.streamingAssetsPath, dbFileName);
        if ( !File.Exists(DatabasePath) )
        {
            File.Create(DatabasePath);
        }

        dbConnection = new SqliteConnection($"Data Source={DatabasePath};Version=3;");
        dbConnection.Open();
    }

    private void CreateTable(string tableName)
    {
        this.tableName = tableName;
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
                    scores.Add(new PlayerScore (
                        id : reader.GetInt32(0),
                        name : reader.GetString(1),
                        score : reader.GetFloat(2),
                        difficulty: reader.GetInt32(3),
                        challengeMode: reader.GetBoolean(4)
                    ));
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
