using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

    private TMP_Text label;
    public string Text
    {
        get { return label.text;  }
        set { label.text = value; }
    }

    private Image image;
    public Color Color
    {
        get { return image.color; }
        set { image.color = value; }
    }

    private int difficulty;
    public int Difficulty
    {
        get { return difficulty; }
        set {
            difficulty = value;
            Color = difficultyColors[value];
            Text = $"x{gameSpeeds[value]}";
        }
    }

    private void Awake()
    {
        if (gameSpeeds == null){
            gameSpeeds ??= GlobalVariables.Configs.Values.Select(config => config.GameSpeed).ToArray();
        }
        toggle = gameObject.GetComponent<Toggle>();
        label = gameObject.GetComponentInChildren<TMP_Text>();
        image = gameObject.GetComponentInChildren<Image>();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    
}
