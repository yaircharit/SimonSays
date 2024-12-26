using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public static class GlobalVariables
    {
        public static AppConfig selectedConfig { get; set; }

        private static string _playerNameKey = "playerName";
        public static string PlayerName
        {
            get { return PlayerPrefs.GetString(_playerNameKey, ""); }
            set { PlayerPrefs.SetString(_playerNameKey, value); }
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

        internal Button buttonRef { get; set; }
    }
}
