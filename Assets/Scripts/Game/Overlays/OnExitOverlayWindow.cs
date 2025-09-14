using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnExitOverlayWindow : OverlayWindow
{
    private float timeScale;
    protected override void OnClose()
    {
        Time.timeScale = timeScale; // Resume game
        SimonSays.Instance.EnableButtons(true);
    }

    protected override void OnOpen()
    {
        timeScale = Time.timeScale;
        Time.timeScale = 0; // Pause game
        SimonSays.Instance.EnableButtons(false);
    }

    public override void OnSubmit()
    {
        // Exit Game
        GameManager.currentGame = null;
        Time.timeScale = 1; 
        SceneManager.LoadScene("GameSetup");
    }

}
