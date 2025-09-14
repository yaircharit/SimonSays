using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRow : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text rankText;
    [SerializeField]
    private TMPro.TMP_Text playerNameText;
    [SerializeField]
    private TMPro.TMP_Text scoreText;
    [SerializeField]
    private ChallengeModeToggle challengeModeToggle;

    public int Rank
    {
        get => int.Parse(rankText.text);
        set => rankText.text = value.ToString();
    }
    public string PlayerName
    {
        get => playerNameText.text;
        set => playerNameText.text = value;
    }
    public float Score
    {
        get => float.Parse(scoreText.text);
        set => scoreText.text = value.ToString();
    }
    public ChallengeModeToggle ChallengeModeToggle
    {
        get => challengeModeToggle;
        set => challengeModeToggle = value;
    }
}
