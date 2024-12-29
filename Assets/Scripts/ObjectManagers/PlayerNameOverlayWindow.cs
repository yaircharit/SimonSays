using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public override void OpenWindow()
    {
        base.OpenWindow();
        textInputField.text = GlobalVariables.PlayerName;

        toggle.IsOn = GlobalVariables.ChallengeMode;    // Restore state from previous game
        toggle.Difficulty = GlobalVariables.SelectedConfigIndex; // Set the difficulty, text, and color
    }

    public override void OnSubmit()
    {
        string playerName = textInputField.text.Trim();

        // Verify the player input
        if ( playerName == "" )
        {
            textInputField.GetComponent<Image>().color = noInputColor;
            textInputField.Select();
            return;
        }

        GlobalVariables.ChallengeMode = toggle.IsOn && GlobalVariables.SelectedConfig.GameSpeed != 1; // not possible when game speed is 1
        GlobalVariables.PlayerName = playerName;
        SceneManager.LoadScene("GameScene");
    }
}
