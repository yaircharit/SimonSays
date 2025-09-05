using System;
using System.Collections.Generic;
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





}

