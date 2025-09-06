using System;

[Serializable]
public class BaseAppConfig
{
    public int Index { get; set; }
}

[Serializable]
public class AppConfig : BaseAppConfig
{
    public string Name { get; set; }
    public int GameButtons { get; set; }
    public int PointsEachStep { get; set; }
    public int GameTime { get; set; }
    public bool RepeatMode { get; set; }
    public float GameSpeed { get; set; }

}
