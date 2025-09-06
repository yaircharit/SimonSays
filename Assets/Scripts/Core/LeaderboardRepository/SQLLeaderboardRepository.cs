using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.LeaderboardRepository
{
    public class SQLLeaderboardRepository : LeaderboardRepository
    {
        private SqliteConnection dbConnection;

        public SQLLeaderboardRepository(string dbFileName, string tableName) : base(dbFileName, tableName)
        {
        }

        protected override void OpenConnection(string dbFileName)
        {
            string DatabasePath = Path.Combine(Application.streamingAssetsPath, dbFileName);
            if (!File.Exists(DatabasePath))
            {
                File.Create(DatabasePath);
            }

            dbConnection = new SqliteConnection($"Data Source={DatabasePath};Version=3;");
            dbConnection.Open();

            //return Task.CompletedTask;
        }

        protected override void CreateTable(string tableName)
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

        public override async Task<List<PlayerScore>> LoadScoresAsync()
        {
            var scores = new List<PlayerScore>();
            using (var command = new SqliteCommand($"SELECT * FROM {tableName} ORDER BY Score DESC", dbConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        scores.Add(new PlayerScore
                        {
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

        public override Task SaveScoreAsync(PlayerScore newScore)
        {
            using var command = new SqliteCommand($"INSERT INTO {tableName} VALUES ({newScore})", dbConnection);
            command.ExecuteNonQuery();
            return Task.CompletedTask;
        }

        public override void CloseConnection()
        {
            dbConnection.Close();
        }
    }
}