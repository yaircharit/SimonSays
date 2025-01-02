using ConfigurationLoader;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This class is used to hold and reference all required variables throughout the game.
    /// Using PlayerPrefs to store settings between sessions
    /// Init must be called to load all needed data from config and database files
    /// </summary>
    public static class GlobalVariables
    { 
        // PlayerPrefs keys
        private const string VolumeKey = "Volume";
        private const string MuteKey = "Mute";
        private const string ResolutionXKey = "ResolutionX";
        private const string ResolutionYKey = "ResolutionY";
        private const string FullscreenKey = "Fullscreen";
        private const string PlayerNameKey = "PlayerName";
        private const string ScoreKey = "Score";
        private const string ChallengeModeKey = "ChallengeModeKey";

        private static string ConfigFileName { get; set; } = "config.json";
        public static string ConfigPath => Path.Combine(Application.streamingAssetsPath, ConfigFileName);
        public static List<AppConfig> Configs { get; set; }

        public static AppConfig SelectedConfig { get; set; }
        public static int SelectedConfigIndex => Configs.IndexOf(SelectedConfig);

        public static bool GameWon { get; set; }

        public static string PlayerName
        {
            get => PlayerPrefs.GetString(PlayerNameKey, ""); // Keep the player's name in the PlayerPrefs, empty string if not set
            set { PlayerPrefs.SetString(PlayerNameKey, value); }
        }

        public static float Score
        {
            get => PlayerPrefs.GetFloat(ScoreKey, -1);    // Last game's score, -1 after exiting back to main menu
            set { PlayerPrefs.SetFloat(ScoreKey, value); }
        }

        public static bool ChallengeMode
        {
            get => PlayerPrefs.GetInt(ChallengeModeKey, 0) == 1;    // If last game was in Challenge Mode or not
            set { PlayerPrefs.SetInt(ChallengeModeKey, value ? 1 : 0); }
        }

        public static float Volume
        {
            get => PlayerPrefs.GetFloat(VolumeKey, 0.5f); // Default volume is 1.0 (100%)
            set { PlayerPrefs.SetFloat(VolumeKey, value); }
        }

        public static bool Mute
        {
            get => PlayerPrefs.GetInt(MuteKey, 0) == 1; // Default is not muted
            set { PlayerPrefs.SetInt(MuteKey, value ? 1 : 0); }
        }

        public static Vector2Int defaultResolution = new Vector2Int(1280, 720);
        public static Resolution DefaultResolution => new Resolution() { width = defaultResolution.x, height = defaultResolution.y };
        public static Resolution Resolution
        {
            get => new() { width = PlayerPrefs.GetInt(ResolutionXKey, DefaultResolution.width), height = PlayerPrefs.GetInt(ResolutionYKey, DefaultResolution.height) };
            set {
                PlayerPrefs.SetInt(ResolutionXKey, value.width);
                PlayerPrefs.SetInt(ResolutionYKey, value.height);
            }
        }

        public static bool Fullscreen
        {
            get => PlayerPrefs.GetInt(FullscreenKey, 1) == 1; // Default is fullscreen
            set { PlayerPrefs.SetInt(FullscreenKey, value ? 1 : 0); }
        }

        /// <summary>
        /// Load config and database if not already loaded
        /// </summary>
        public static void Init(string configFileName = "config.json", string dbFileName = "leaderboard.db", string tableName = "Leaderboard")
        {
            if ( Configs == null )
            {
                ConfigFileName = configFileName;
                Configs = ConfigLoader<AppConfig>.LoadConfig(ConfigPath);

                Leaderboard.Init(dbFileName, tableName);
                SettingsWindow.LoadSettings();
            }
        }
    }
}
