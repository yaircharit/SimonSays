using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNameOverlayWindow : OverlayWindow
{
    public TMPro.TMP_InputField textInputField;
    public Color noInputColor = new Color(243, 183, 183);

    public void Awake()
    {
        CloseWindow();
        textInputField.text = GlobalVariables.PlayerName;
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

        GlobalVariables.PlayerName = playerName;
        SceneManager.LoadScene("GameScene");
    }
}


