using Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.LeaderboardRepository
{
    public abstract class LeaderboardRepository
    {
        protected string dbFileName;
        protected string tableName;

        protected LeaderboardRepository(string dbFileName, string tableName)
        {
            this.dbFileName = dbFileName;
            this.tableName = tableName;
            OpenConnection(dbFileName);
            CreateTable(tableName);
        }

        protected abstract void OpenConnection(string dbFileName);
        protected abstract void CreateTable(string tableName);
        public abstract Task<List<PlayerScore>> LoadScoresAsync();
        public abstract Task SaveScoreAsync(PlayerScore newScore);
        public abstract void CloseConnection();


        public static LeaderboardRepository CreateRepository(string dbFileName = null, string tableName = null)
        {
            dbFileName ??= SettingsManager.Settings.databaseFilePath;
            tableName ??= SettingsManager.Settings.leaderboardTableName;

            return Path.GetExtension(dbFileName) switch
            {
                ".db" => new SQLLeaderboardRepository(dbFileName, tableName),
                ".firebase" => new FirebaseLeaderboardRepository(dbFileName.Substring(0, dbFileName.Length - ".firebase".Length), tableName),
                _ => throw new ArgumentException("Invalid repository type"),
            };
        }


    }
}

