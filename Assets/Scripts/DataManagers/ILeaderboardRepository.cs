using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class ILeaderboardRepository
{
    protected string dbFileName;
    protected string tableName;

    protected ILeaderboardRepository(string dbFileName, string tableName)
    {
        this.dbFileName = dbFileName;
        this.tableName = tableName;
        OpenConnection( dbFileName);
        CreateTable(tableName);
    }

    protected abstract void OpenConnection(string dbFileName);
    protected abstract void CreateTable(string tableName);
    public abstract Task<List<PlayerScore>> LoadScoresAsync();
    public abstract Task SaveScoreAsync(PlayerScore newScore);
    public abstract void CloseConnection();


    public static ILeaderboardRepository CreateRepository(string dbFileName = "leaderboard.db", string tableName = "Leaderboard")
    {
        return Path.GetExtension(dbFileName) switch
        {
            ".db" => new SQLLeaderboardRepository(dbFileName, tableName),
            ".firebase" => new FirebaseLeaderboardRepository(Path.GetFileNameWithoutExtension(dbFileName), tableName),
            _ => throw new ArgumentException("Invalid repository type"),
        };
    }


}

