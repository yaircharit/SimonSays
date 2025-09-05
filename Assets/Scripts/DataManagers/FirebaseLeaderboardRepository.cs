using ConfigurationLoader;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseLeaderboardRepository : ILeaderboardRepository
{
    public FirebaseLeaderboardRepository(string dbFileName, string tableName) : base(dbFileName, tableName)
    {

    }

    public override void CloseConnection()
    {
        //throw new NotImplementedException();
    }

    public override async Task<List<PlayerScore>> LoadScoresAsync()
    {
        var snapshot = await FirebaseRestAPI.Get($"{dbFileName}/{tableName}");
        var leaderboard = new List<PlayerScore>();
        Debug.Log(snapshot);
        if (snapshot != null && snapshot != "")
        {
            var snapshotObj = JsonConvert.DeserializeObject<Dictionary<string,object>>(snapshot);
            foreach (var child in snapshotObj.Values)
            {
                try
                {
                    var entry = JsonConvert.DeserializeObject<PlayerScore>(child.ToString());
                    leaderboard.Add(entry);
                }
                catch (System.Exception)
                {
                    Debug.LogError($"Failed to parse leaderboard entry: {child}");
                }
            }
        }

        // Firebase returns ascending order, so reverse if needed
        leaderboard.Sort((a, b) => b.Score.CompareTo(a.Score));
        return leaderboard;
    }

    public async override Task SaveScoreAsync(PlayerScore entry)
    {
        // Push a new entry (auto-generated key)
        string json = JsonConvert.SerializeObject(entry);
        await FirebaseRestAPI.Put($"{dbFileName}/{tableName}", json);

    }


    protected override void CreateTable(string tableName)
    {
        //throw new NotImplementedException();
    }

    protected override void OpenConnection(string dbFileName)
    {
        //_dbRef = FirebaseDatabase.DefaultInstance.RootReference.Child($"{dbFileName}/{tableName}");
    }
}
