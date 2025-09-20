using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// An overlay that also contains a text input for player's name and a toggle button to enable/disable ChallengeMode
/// </summary>
public class PlayerNameOverlayWindow : OverlayWindow
{
    [SerializeField]
    private TMPro.TMP_InputField textInputField;
    [SerializeField]
    private Color noInputColor = new Color(243, 183, 183);

    [SerializeField]
    private ChallengeModeToggle toggle;

    protected override void OnOpen()
    {
        // Restore name and state from previous game
        textInputField.text = GameSetup.PlayerName;
        toggle.IsOn = GameSetup.ChallengeMode;

        // Set the difficulty, text, and color to reflect the selected difficulty
        toggle.SetDifficulty(GameSetup.SelectedConfig);
    }

    public override void OnSubmit()
    {
        string playerName = textInputField.text.Trim();

        // Require player input
        if (playerName == "")
        {
            //TODO: must be better way than change thw color of the Image component
            textInputField.GetComponent<Image>().color = noInputColor;
            textInputField.Select();
            return;
        }

        GameSetup.ChallengeMode = toggle.IsOn && GameSetup.SelectedConfig.GameSpeed != 1; // not possible when game speed is 1
        GameSetup.PlayerName = playerName;
        SceneManager.LoadScene("GameScene");
    }
}
