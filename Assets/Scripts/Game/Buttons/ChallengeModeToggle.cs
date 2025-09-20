using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeModeToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    public bool IsOn
    {
        get { return toggle.isOn; }
        set { toggle.isOn = value; }
    }
    public bool Interactable
    {
        get { return toggle.interactable; }
        set { toggle.interactable = value; }
    }

    [SerializeField]
    private TMP_Text label;
    public string Text
    {
        get { return label.text; }
        set { label.text = value; }
    }

    [SerializeField]
    private Image image;
    public UnityEngine.Color32 Color
    {
        get { return image.color; }
        set { image.color = value; }
    }

    [SerializeField]
    private Image checkmarkImage;
    public UnityEngine.Color32 checkmarkColor
    {
        get { return checkmarkImage.color; }
        set { checkmarkImage.color = value; }
    }

    public AppConfig difficulty;

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
    public void SetDifficulty(AppConfig difficulty)
    {
        this.difficulty = difficulty;
        Color = difficulty.PrimaryColor;
        checkmarkColor = difficulty.SecondaryColor;
        Text = $"x{difficulty.GameSpeed}"; // x1.5 or x1.25 ...

        if (difficulty.GameSpeed == 1)
        {
            label.gameObject.SetActive(false);
            IsOn = Interactable = false; //disable. no challenge mode when game speed is 1
        }
        else
        {
            label.gameObject.SetActive(true);
            Interactable = true;
            IsOn = GameSetup.ChallengeMode; //restore previous state
        }
    }

    public void SetDifficulty(int difficultyIndex)
    {
        SetDifficulty(Core.Configs.ConfigManager<AppConfig>.Configs[difficultyIndex]);
    }
}