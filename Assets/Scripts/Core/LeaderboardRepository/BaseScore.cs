using Mono.Data.Sqlite;
using Newtonsoft.Json;

[System.Serializable]
public class BaseScore
{
    [JsonProperty("PlayerName")]
    public string PlayerName { get; set; }
    [JsonProperty("Score")]
    public float Score { get; set; }

    public BaseScore()
    {
        PlayerName = null;
        Score = 0;
    }

    public virtual void LoadFromReader(SqliteDataReader reader)
    {
        PlayerName = reader.GetString(1);
        Score = reader.GetFloat(2);
    }
}

