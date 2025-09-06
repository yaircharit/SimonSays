using Core.Configs;
using Core.LeaderboardRepository;
using Core.Network.Firebase;
using Core.Settings;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public static class Bootstrapper<T1, T2> where T1 : BaseAppConfig, new() where T2 : BaseScore, new()
    {
        public static async Task InitializeAsync()
        {
            string dbFileName = SettingsManager.Settings.dbFilePath;
            string configPath = SettingsManager.Settings.configPath;
            
            // Ensure Firebase login
            var user = await FirebaseRestAPI.Login();

            // If login fails, use fallback settings
            if (user == null)
            {
                dbFileName = SettingsManager.Settings.fallbackDbFilePath;
                configPath = SettingsManager.Settings.fallbackConfigPath;
            }

            // Load configs
            await ConfigManager<T1>.LoadConfigsAsync(configPath);

            // Initialize leaderboard repository
            await LeaderboardRepository<T2>.InitializeAsync(dbFileName);

            Debug.Log("Initialization complete!");
        }
    }
}
