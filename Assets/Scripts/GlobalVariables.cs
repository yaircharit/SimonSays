using ConfigurationLoader;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GlobalVariables : ScriptableObject
    {
        public static GlobalVariables Instance = CreateInstance<GlobalVariables>();

        public Vector2Int defaultResolution = new Vector2Int(1280, 720);
        public string configName = "config.json";
        

        public static Resolution DefaultResolution => new Resolution() { width = Instance.defaultResolution.x, height = Instance.defaultResolution.y };
        public static string ConfigPath => Path.Combine(Application.streamingAssetsPath, Instance.configName);
        
        public static Dictionary<string, AppConfig> Configs;

        public static AppConfig SelectedConfig { get; set; }
        public static bool GameWon { get; set; }


        private const string VolumeKey = "Volume";
        private const string MuteKey = "Mute";
        private const string ResolutionXKey = "ResolutionX";
        private const string ResolutionYKey = "ResolutionY";
        private const string FullscreenKey = "Fullscreen";
        private const string PlayerNameKey = "PlayerName";
        private const string ScoreKey = "Score";

        public static string PlayerName
        {
            get => PlayerPrefs.GetString(PlayerNameKey, ""); // Keep the player's name in the PlayerPrefs, empty string if not set
            set { PlayerPrefs.SetString(PlayerNameKey, value); }
        }

        public static int Score
        {
            get => PlayerPrefs.GetInt(ScoreKey, -1);    // Last game's score, -1 after exiting back to main menu
            set { PlayerPrefs.SetInt(ScoreKey, value); }
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
    }
}
