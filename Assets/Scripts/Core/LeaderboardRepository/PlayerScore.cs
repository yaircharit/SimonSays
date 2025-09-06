using Newtonsoft.Json;

[System.Serializable]
public class PlayerScore
{
    [JsonProperty("Id")]
    public int Id { get; set; }
    [JsonProperty("PlayerName")]
    public string PlayerName { get; set; }
    [JsonProperty("Score")]
    public float Score { get; set; }
    [JsonProperty("Difficulty")]
    public int Difficulty { get; set; }
    [JsonProperty("Challenge")]
    public bool Challenge { get; set; }
    [System.NonSerialized]
    public bool gameWon;
}

