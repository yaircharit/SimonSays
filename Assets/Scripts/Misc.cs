using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public static class GlobalVariables
    {
        public static Dictionary<string, AppConfig> Configs { get; set; }
        public static AppConfig SelectedConfig { get; set; }
        public static bool GameWon { get; set; }

        private const string VolumeKey = "Volume";
        private const string MuteKey = "Mute";
        private const string ResolutionKey = "Resolution";
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
            get => PlayerPrefs.GetInt(ScoreKey, -1);    // Last game's score, -1 if no game was played
            set { PlayerPrefs.SetInt(ScoreKey, value); }
        }

        public static float Volume
        {
            get => PlayerPrefs.GetFloat(VolumeKey, 1.0f); // Default volume is 1.0 (100%)
            set { PlayerPrefs.SetFloat(VolumeKey, value); }
        }

        public static bool Mute
        {
            get => PlayerPrefs.GetInt(MuteKey, 0) == 1; // Default is not muted
            set { PlayerPrefs.SetInt(MuteKey, value ? 1 : 0); }
        }

        public static int Resolution
        {
            get => PlayerPrefs.GetInt(ResolutionKey, 0); // Default resolution index is 0
            set { PlayerPrefs.SetInt(ResolutionKey, value); }
        }

        public static bool Fullscreen
        {
            get => PlayerPrefs.GetInt(FullscreenKey, 1) == 1; // Default is fullscreen
            set { PlayerPrefs.SetInt(FullscreenKey, value ? 1 : 0); }
        }
    }

    [Serializable]
    public class AppConfig
    {
        public int GameButtons { get; set; }
        public int PointsEachStep { get; set; }
        public int GameTime { get; set; }
        public bool RepeatMode { get; set; }
        public float GameSpeed { get; set; }

    }
}
