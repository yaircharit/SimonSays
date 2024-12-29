using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnExitOverlayWindow : OverlayWindow
{
    public override void OnSubmit()
    {
        SceneManager.LoadScene("GameSetup");
    }

}
