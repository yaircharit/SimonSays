using Mono.Data.Sqlite;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerScore : BaseScore
{
    [JsonProperty("Difficulty")]
    public int Difficulty { get; set; }
    [JsonProperty("Challenge")]
    public bool Challenge { get; set; }
    [System.NonSerialized]
    public bool gameWon;

    public PlayerScore() : base()
    {
        Difficulty = 0;
        Challenge = false;
        gameWon = false;
    }


    public override void LoadFromReader(SqliteDataReader reader)
    {
        base.LoadFromReader(reader);
        Difficulty = reader.GetInt32(3);
        Challenge = reader.GetBoolean(4);
    }
}

