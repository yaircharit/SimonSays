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


    public override void OpenWindow()
    {
        base.OpenWindow();
        textInputField.text = GlobalVariables.PlayerName;
        
        toggle = challengeModeToggleGameObject.GetComponent<ChallengeModeToggle>();

        toggle.SetActive(GlobalVariables.SelectedConfig.GameSpeed != 1); // Show only if there's a difference
        toggle.IsOn = GlobalVariables.ChallengeMode;
        toggle.Difficulty = GlobalVariables.SelectedConfigIndex;
    }

    public override void OnSubmit()
    {
        string playerName = textInputField.text.Trim();

        if ( playerName == "" )
        {
            textInputField.GetComponent<Image>().color = noInputColor;
            textInputField.Select();
            return;
        }

        GlobalVariables.ChallengeMode = toggle.IsOn && GlobalVariables.SelectedConfig.GameSpeed != 1;
        GlobalVariables.PlayerName = playerName;
        SceneManager.LoadScene("GameScene");
    }
}
