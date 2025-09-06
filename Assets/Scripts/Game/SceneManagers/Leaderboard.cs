using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Core.LeaderboardRepository;


// Can also be a generic and abstract class but theres no real need for that (Leaderboard<PlayerScore> : Table<T> : OverlayWindwo (even less neccessary) : MonoBehaviour)
public class Leaderboard : MonoBehaviour
{
    public GameObject rowPrefab;
    public GameObject rowsContainer;
    public Color hightlightColor = Color.yellow;

    private TMP_Text titleTextObject;
    private static List<PlayerScore> playerScores => repository.values.OrderByDescending(score => score.Score).ToList();
    private Dictionary<PlayerScore, GameObject> rows;
    public static PlayerScore lastGame;
    private static LeaderboardRepository<PlayerScore> repository => LeaderboardRepository<PlayerScore>.Instance;
    private string nextScene = "GameSetup";

    private void Start()
    {
        titleTextObject = transform.Find("WindowTitle").GetComponent<TMP_Text>();

        if ( lastGame != null )
        {
            repository.SaveScoreAsync(lastGame);
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

            rows[score] = row;
        }

        if ( lastGame != null ) // If the last game is over (win/lose)
        {
            titleTextObject.text = lastGame.gameWon ? "Congratulations!" : "You Lost!";
            // TODO: add sounds (win / lose)
            HightlightRow(lastGame);
            lastGame = null;
        }
    }

    public void CloseWindow()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void HightlightRow(PlayerScore playerScore)
    {
        rows[playerScore].GetComponent<Image>().color = hightlightColor;
        ScrollTo(playerScore);
    }

    public void ScrollTo(PlayerScore playerScore)
    {
        Canvas.ForceUpdateCanvases();
        RectTransform containerRectTransform = rowsContainer.GetComponent<RectTransform>();
        RectTransform rowRectTransform = rows[playerScore].GetComponent<RectTransform>();

        Vector2 rowPosition = (Vector2)containerRectTransform.InverseTransformPoint(containerRectTransform.position) - (Vector2)containerRectTransform.InverseTransformPoint(rowRectTransform.position);
        float rowHeight = rowRectTransform.rect.height;

        containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, rowPosition.y - (rowHeight * 10));
    }

    public void OnApplicationQuit()
    {
        repository.CloseConnection();
    }
}
