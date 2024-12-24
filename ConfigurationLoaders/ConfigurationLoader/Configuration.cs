using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLoader
{
    public class Configuration
    {
        public required string Name { get; set; }
        public int GameButtons { get; set; }
        public int PointsEachStep { get; set; }
        public int GameTime { get; set; }
        public bool RepeatMode { get; set; }
        public float GameSpeed { get; set; }
    }
}
