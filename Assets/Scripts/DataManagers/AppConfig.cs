using System;

namespace Assets.Scripts
{
    [Serializable]
    public class AppConfig
    {
        public string Name { get; set; }
        public int GameButtons { get; set; }
        public int PointsEachStep { get; set; }
        public int GameTime { get; set; }
        public bool RepeatMode { get; set; }
        public float GameSpeed { get; set; }

    }
}
