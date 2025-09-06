using System;


[Serializable]
public class AppConfig : BaseAppConfig
{
    public int GameButtons { get; set; }
    public int PointsEachStep { get; set; }
    public int GameTime { get; set; }
    public bool RepeatMode { get; set; }
    public float GameSpeed { get; set; }

}