using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// An overlay that also contains a text input for player's name and a toggle button to enable/disable ChallengeMode
/// </summary>
public class PlayerNameOverlayWindow : OverlayWindow
{
    public TMPro.TMP_InputField textInputField;
    public Color noInputColor = new Color(243, 183, 183);
    
    public GameObject challengeModeToggleGameObject;
    private ChallengeModeToggle toggle;

    private void Awake()
    {
        toggle = challengeModeToggleGameObject.GetComponent<ChallengeModeToggle>();
    }

    protected override void OnOpen()
    {
        // Restore name and state from previous game
        textInputField.text = GameSetup.PlayerName;
        toggle.IsOn = GameSetup.ChallengeMode;

        // Set the difficulty, text, and color to reflect the selected difficulty
        toggle.SetDifficulty(GameSetup.SelectedConfigIndex); 
    }

    public override void OnSubmit()
    {
        string playerName = textInputField.text.Trim();

        // Require player input
        if ( playerName == "" )
        {
            textInputField.GetComponent<Image>().color = noInputColor;
            textInputField.Select();
            return;
        }

        GameSetup.ChallengeMode = toggle.IsOn && GameSetup.SelectedConfig.GameSpeed != 1; // not possible when game speed is 1
        GameSetup.PlayerName = playerName;
        SceneManager.LoadScene("GameScene");
    }
}
