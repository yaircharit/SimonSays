using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Core.Network.Firebase;

namespace Core.LeaderboardRepository
{
    public class FirebaseLeaderboardRepository<T> : LeaderboardRepository<T> where T : BaseScore, new()
    {
        public FirebaseLeaderboardRepository(string dbFileName, string tableName) : base(dbFileName, tableName) { }

        public override void CloseConnection()
        {
            //throw new NotImplementedException();
        }

        public override async Task<List<T>> LoadScoresAsync()
        {
            var snapshot = await FirebaseRestAPI.Get(FirebaseRestAPI.GetURL($"{dbFileName}/{tableName}"));
            var leaderboard = new List<T>();
            if (snapshot != null && snapshot != "")
            {
                var snapshotObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot);
                foreach (var child in snapshotObj.Values)
                {
                    try
                    {
                        var entry = JsonConvert.DeserializeObject<T>(child.ToString());
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

        public async override Task SaveScoreAsync(T entry)
        {
            _ =base.SaveScoreAsync(entry); // Add to local list
            // Push a new entry (auto-generated key)
            string json = JsonConvert.SerializeObject(entry);
            await FirebaseRestAPI.Post(FirebaseRestAPI.GetURL($"{dbFileName}/{tableName}"), json);
        }


        protected override void CreateTable(string tableName)
        {
            //throw new NotImplementedException();
        }

        protected override void OpenConnection(string dbFileName)
        {

        }
    }
}