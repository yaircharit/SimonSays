using Assets.Scripts;
using System.Linq;
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

    public Color[] checkmarkColors = {
        new Color(255, 255, 255, 255), // White
        new Color(255,200,0,255),  // Gold
        new Color(160,160,160,255) // Silver
    };

    internal static float[] gameSpeeds;

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

    public int difficulty;

    private ChallengeModeState currentState;

    private void Awake()
    {
        if ( gameSpeeds == null )
        {
            // Save all possible game speeds
            gameSpeeds = GlobalVariables.Configs.Select(config => config.GameSpeed).ToArray();
        }
        toggle = gameObject.GetComponent<Toggle>();
        label = gameObject.GetComponentInChildren<TMP_Text>();
        image = gameObject.GetComponentInChildren<Image>();
        checkmarkImage = gameObject.transform.Find("Background").GetChild(0).GetComponent<Image>();

        currentState = new DisabledState(this);
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
        checkmarkColor = checkmarkColors[difficulty];

        if ( gameSpeeds[difficulty] == 1 )
        {
            currentState = new DisabledState(this);
        } else
        {
            currentState = new EnabledState(this);
        }

        return currentState.Handle(difficulty);
    }
}

public abstract class ChallengeModeState
{
    protected ChallengeModeToggle toggle;
    public ChallengeModeState(ChallengeModeToggle toggle)
    {
        this.toggle = toggle;
    }
    public abstract bool Handle(int difficulty);
}

public class DisabledState : ChallengeModeState
{
    public DisabledState(ChallengeModeToggle toggle) : base(toggle)
    {
    }

    public override bool Handle(int difficulty)
    {
        toggle.Text = ""; // Don't print text if it's disabled (x1)
        toggle.IsOn = toggle.interactable = false; //disable
        return toggle.interactable;
    }
}

public class EnabledState : ChallengeModeState
{
    public EnabledState(ChallengeModeToggle toggle) :base(toggle)
    {
    }

    public override bool Handle(int difficulty)
    {
        toggle.Text = $"x{ChallengeModeToggle.gameSpeeds[difficulty]}"; // x1.5 or x1.25 ...
        toggle.interactable = true;
        return toggle.interactable;
    }
}
