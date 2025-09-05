using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// Can also be a generic and abstract class but theres no real need for that (Leaderboard<PlayerScore> : Table<T> : OverlayWindwo (even less neccessary) : MonoBehaviour)
public class Leaderboard : MonoBehaviour
{
    public string databaseFileName = "SimonSays.firebase";
    public string tableName = "Leaderboard";
    public GameObject rowPrefab;
    public GameObject rowsContainer;
    public Color hightlightColor = Color.yellow;

    private TMP_Text titleTextObject;
    private static List<PlayerScore> playerScores;
    private Dictionary<int, GameObject> rows;
    public static PlayerScore lastGame;
    private static ILeaderboardRepository repository;
    public static int Count => playerScores?.Count ?? 0;
    private string nextScene = "GameSetup";

    private static Coroutine loaderCoroutine;

    void Awake()
    {
        // Initialize the repository and load the scores
        repository ??= LeaderboardRepositoryFactory.CreateRepository(databaseFileName, tableName);
        playerScores ??= new();
        loaderCoroutine = StartCoroutine(LoadScoresAsync());
    }

    private IEnumerator LoadScoresAsync()
    {
        var loadTask = repository.LoadScoresAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);
        playerScores = loadTask.Result;
    }

    private IEnumerator Start()
    {
        yield return loaderCoroutine; // Wait for the loading to complete


        titleTextObject = transform.Find("WindowTitle").GetComponent<TMP_Text>();

        if ( lastGame != null )
        {
            SaveScoreAsync(lastGame);
            nextScene = "GameSetup";
        } else
        {
            nextScene = "MainMenu";
        }

        rows = new();
        DisplayScores();
    }


    private void DisplayScores()
    {
        int rank = 1;

        foreach ( var score in playerScores )
        {
            GameObject row = Instantiate(rowPrefab, rowsContainer.transform);
            var textComponents = row.GetComponentsInChildren<TextMeshProUGUI>();

            textComponents[0].text = rank++.ToString(); // Rank
            textComponents[1].text = score.PlayerName; // Player Name
            textComponents[3].text = score.Score.ToString(); // Score. Skipping index=2 cuz of Text component in Challenge Mode object

            var difficultyComponent = row.GetComponentInChildren<ChallengeModeToggle>();
            difficultyComponent.SetDifficulty(score.Difficulty); // Set difficulty level and color
            difficultyComponent.IsOn = score.Challenge; // Set challanage mode indication
            difficultyComponent.interactable = false;

            rows[score.Id] = row;
        }

        if ( lastGame != null ) // If the last game is over (win/lose)
        {
            titleTextObject.text = lastGame.gameWon ? "Congratulations!" : "You Lost!";
            // TODO: add sounds (win / lose)
            HightlightRow(lastGame);
            lastGame = null;
        }
    }

    private void SaveScoreAsync(PlayerScore newScore)
    {
        newScore.Id = Count + 1;
        playerScores.Add(newScore);
        playerScores = playerScores.OrderByDescending(score => score.Score).ToList();
        repository.SaveScoreAsync(newScore);
    }

    public void CloseWindow()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void HightlightRow(PlayerScore playerScore)
    {
        rows[playerScore.Id].GetComponent<Image>().color = hightlightColor;
        ScrollTo(playerScore);
    }

    public void ScrollTo(PlayerScore playerScore)
    {
        Canvas.ForceUpdateCanvases();
        RectTransform containerRectTransform = rowsContainer.GetComponent<RectTransform>();
        RectTransform rowRectTransform = rows[playerScore.Id].GetComponent<RectTransform>();

        Vector2 rowPosition = (Vector2)containerRectTransform.InverseTransformPoint(containerRectTransform.position) - (Vector2)containerRectTransform.InverseTransformPoint(rowRectTransform.position);
        float rowHeight = rowRectTransform.rect.height;

        containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, rowPosition.y - (rowHeight * 10));
    }

    public void OnApplicationQuit()
    {
        repository.CloseConnection();
    }
}

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

    public PlayerScore()
    {
        Id = Leaderboard.Count + 1;
        PlayerName = GameSetup.PlayerName;
        Score = 0;
        Difficulty = GameSetup.SelectedConfigIndex;
        Challenge = GameSetup.ChallengeMode;
    }

    public PlayerScore(int id, string name, float score, int difficulty, bool challengeMode)
    {
        Id = id;
        PlayerName = name;
        Score = score;
        Difficulty = difficulty;
        Challenge = challengeMode;
    }

    public override string ToString()
    {
        return $"{Id}, '{PlayerName}', {Score}, {Difficulty}, {(Challenge ? 1 : 0)}";
    }
}
