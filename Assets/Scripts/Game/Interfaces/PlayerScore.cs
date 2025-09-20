using Mono.Data.Sqlite;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerScore : BaseScore
{
    [JsonProperty("Difficulty")]
    public int DifficultyIndex;
    [JsonProperty("Challenge")]
    public bool Challenge { get; set; }
    [System.NonSerialized]
    public bool gameWon;

    public PlayerScore() : this(null, 0, -1, false, false) { }

    public PlayerScore(string PlayerName = null, int Score = 0, int Difficulty = -1, bool Challenge = false, bool Win = false) : base(PlayerName, Score)
    {
        this.Challenge = Challenge;
        gameWon = Win;
        DifficultyIndex = Difficulty;
    }

    public override void LoadFromReader(SqliteDataReader reader)
    {
        base.LoadFromReader(reader);
        DifficultyIndex = reader.GetInt32(3);
        Challenge = reader.GetBoolean(4);
    }
}

