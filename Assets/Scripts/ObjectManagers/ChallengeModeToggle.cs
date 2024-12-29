using Assets.Scripts;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeModeToggle : MonoBehaviour
{
    public Color[] difficultyColors = {
        new Color(175,100,50,255), // Bronze
        new Color(160,160,160,255), // Silver
        new Color(255,200,0,255)  // Gold
    };

    private static float[] gameSpeeds;

    private Toggle toggle;
    public bool IsOn
    {
        get { return toggle.isOn; }
        set { toggle.isOn = value; }
    }
    public bool interactable
    {
        get { return toggle.interactable; }
        set { toggle.interactable = value; }
    }

    private TMP_Text label;
    public string Text
    {
        get { return label.text; }
        set { label.text = value; }
    }

    private Image image;
    public Color Color
    {
        get { return image.color; }
        set { image.color = value; }
    }

    private Image checkmarkImage;
    public Color checkmarkColor
    {
        get { return checkmarkImage.color; }
        set { checkmarkImage.color = value; }
    }

    private int difficulty;
    public int Difficulty
    {
        get { return difficulty; }
        set { SetDifficulty(value); }
    }

    private void Awake()
    {
        if ( gameSpeeds == null )
        {
            // Save all possible game speeds
            gameSpeeds = GlobalVariables.Configs.Values.Select(config => config.GameSpeed).ToArray();
        }
        toggle = gameObject.GetComponent<Toggle>();
        label = gameObject.GetComponentInChildren<TMP_Text>();
        image = gameObject.GetComponentInChildren<Image>();
        checkmarkImage = gameObject.transform.Find("Background").GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// Sets the toggle object state
    /// </summary>
    /// <param name="active">new state</param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    /// <summary>
    /// Sets the difficulty and adjusts the color and text accordingly (no text and no interactablity, for x1)
    /// </summary>
    /// <param name="difficulty"></param>
    /// <returns>if the button is interactable</returns>
    public bool SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
        Color = difficultyColors[difficulty];
        if ( gameSpeeds[difficulty] == 1 )
        {
            Text = ""; // Don't print text if it's x1...
            toggle.isOn = toggle.interactable = false; //disable
        } else
        {
            Text = $"x{gameSpeeds[difficulty]}"; // x1.5
            toggle.interactable = true;
            checkmarkColor = difficultyColors[difficulty % 2 + 1]; // For silver trophy (Medium) set star to gold, For gold set it to silver
        }

        return toggle.interactable;
    }
}
