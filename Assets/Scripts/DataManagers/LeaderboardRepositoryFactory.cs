using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class LeaderboardRepositoryFactory
{
    public enum RepositoryType
    {
        SQL,
        Firebase
    }
    public static ILeaderboardRepository CreateRepository(string dbFileName = "leaderboard.db", string tableName = "Leaderboard")
    {
        UnityEngine.Debug.Log($"Loading from {dbFileName}/{tableName}");

        return Path.GetExtension(dbFileName) switch
        {
            ".db" => new SQLLeaderboardRepository(dbFileName, tableName),
            ".firebase" => new FirebaseLeaderboardRepository(Path.GetFileNameWithoutExtension(dbFileName),tableName),
            _ => throw new ArgumentException("Invalid repository type"),
        };
    }
}
