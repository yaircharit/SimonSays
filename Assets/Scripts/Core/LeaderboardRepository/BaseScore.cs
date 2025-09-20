[System.Serializable]
public class BaseScore
{
    [Newtonsoft.Json.JsonProperty("PlayerName")]
    public string PlayerName { get; set; }
    [Newtonsoft.Json.JsonProperty("Score")]
    public float Score { get; set; }

    public BaseScore(string PlayerName = null, int Score = 0)
    {
        this.PlayerName = PlayerName;
        this.Score = Score;
    }

    public virtual void LoadFromReader(Mono.Data.Sqlite.SqliteDataReader reader)
    {
        PlayerName = reader.GetString(1);
        Score = reader.GetFloat(2);
    }
}

