using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseLeaderboardRepository : ILeaderboardRepository
{
    private DatabaseReference _dbRef;

    public FirebaseLeaderboardRepository(string dbFileName, string tableName) : base(dbFileName, tableName)
    {

    }

    public override void CloseConnection()
    {
        //throw new NotImplementedException();
    }

    public override async Task<List<PlayerScore>> LoadScoresAsync()
    {
        var snapshot = await _dbRef.GetValueAsync();

        var leaderboard = new List<PlayerScore>();

        //Debug.Log(snapshot.);

        if (snapshot.Exists)
        {
            foreach (var child in snapshot.Children.Where((ch)=>ch.Value != null))
            {
                //Debug.Log(child.GetRawJsonValue());
                try
                {
                    var entry = JsonConvert.DeserializeObject<PlayerScore>(child.GetRawJsonValue());
                    leaderboard.Add(entry);
                }
                catch (System.Exception)
                {
                    Debug.LogError($"Failed to parse leaderboard entry: {child.GetRawJsonValue()}");
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
        string key = _dbRef.Push().Key;
        string json = JsonConvert.SerializeObject(entry);
        await _dbRef.Child(key).SetRawJsonValueAsync(json);

    }


    protected override void CreateTable(string tableName)
    {
        //throw new NotImplementedException();
    }

    protected override void OpenConnection(string dbFileName)
    {
        _dbRef = FirebaseDatabase.DefaultInstance.RootReference.Child($"{dbFileName}/{tableName}");
    }
}
