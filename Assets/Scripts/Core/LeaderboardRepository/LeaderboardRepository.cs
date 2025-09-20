using Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.LeaderboardRepository
{
    public abstract class LeaderboardRepository<T> where T : BaseScore, new()
    {
        public static LeaderboardRepository<T> Instance { get; private set; }


        protected string dbFileName;
        protected string tableName;

        public List<T> Values { get; private set; }

        protected LeaderboardRepository(string dbFileName, string tableName)
        {
            this.dbFileName = dbFileName;
            this.tableName = tableName;
            this.Values = new List<T>();

            OpenConnection(dbFileName);
            CreateTable(tableName);
        }

        protected abstract void OpenConnection(string dbFileName);
        protected abstract void CreateTable(string tableName);
        public abstract Task<List<T>> LoadScoresAsync();
        public virtual Task SaveScoreAsync(T newScore)
        {
            Values.Add(newScore);
            return Task.CompletedTask;
        }
        public abstract void CloseConnection();

        public static async Task InitializeAsync(string dbFileName = null, string tableName = null)
        {
            Instance ??= CreateRepository(dbFileName, tableName);
            var scores = await Instance.LoadScoresAsync();
            Instance.Values = scores;
            Debug.Log($"[Leaderboard] Loaded {scores.Count} scores");
        }

        public static LeaderboardRepository<T> CreateRepository(string dbFileName = null, string tableName = null)
        {
            dbFileName ??= SettingsManager.Settings.dbFilePath;
            tableName ??= SettingsManager.Settings.tableName;

            return Path.GetExtension(dbFileName) switch
            {
                ".db" => new SQLLeaderboardRepository<T>(dbFileName, tableName),
                ".firebase" => new FirebaseLeaderboardRepository<T>(dbFileName.Substring(0, dbFileName.Length - ".firebase".Length), tableName),
                _ => throw new ArgumentException("Invalid repository type"),
            };
        }


    }
}

