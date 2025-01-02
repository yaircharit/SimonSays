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
    public GameObject rowPrefab;
    public TMP_Text titleTextObject;
    public GameObject rowsContainer;
    public Color hightlightColor = Color.yellow;

    private static List<PlayerScore> playerScores;
    private Dictionary<int, GameObject> rows;
    private PlayerScore lastGame;
    private static LeaderboardRepository repository;

    void Awake()
    {
        if ( GameManager.Score != -1 )
        {
            lastGame = SaveScore();
        }

        rows = new();
        DisplayScores();
    }

    public static void Init(string dbFileName, string tableName)
    {
            repository ??= LeaderboardRepository.Init(dbFileName, tableName);
            playerScores ??= repository.LoadScores();
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

        if ( GameManager.Score != -1 ) // If the last game is over (win/lose)
        {
            titleTextObject.text = GameManager.GameWon ? "Congratulations!" : "You Lost!";
            // TODO: add sounds (win / lose)
            HightlightRow(lastGame);
            GameManager.Score = -1;
        }
    }

    public PlayerScore SaveScore()
    {
        return SaveScore(new PlayerScore() {
            Id = playerScores.Count + 1,
            PlayerName = GameSetup.PlayerName,
            Score = GameManager.Score,
            Difficulty = GameSetup.SelectedConfigIndex,
            Challenge = GameManager.ChallengeMode
        });
    }

    private PlayerScore SaveScore(PlayerScore newScore)
    {
        repository.SaveScore(newScore);
        playerScores.Add(newScore);
        playerScores = playerScores.OrderByDescending(score => score.Score).ToList();
        return newScore;
    }

    public void CloseWindow()
    {
        SceneManager.LoadScene("GameSetup");
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
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public float Score { get; set; }
    public int Difficulty { get; set; }
    public bool Challenge { get; set; }

    public override string ToString()
    {
        return $"{Id}, '{PlayerName}', {Score}, {Difficulty}, {(Challenge ? 1 : 0)}";
    }
}
