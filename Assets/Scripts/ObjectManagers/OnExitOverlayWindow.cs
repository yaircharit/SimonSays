using Assets.Scripts;
using UnityEngine.SceneManagement;

public class OnExitOverlayWindow : OverlayWindow
{
    protected override void OnClose()
    {
        ViewManager.EnableButtons(true);
    }

    protected override void OnOpen()
    {
        ViewManager.EnableButtons(false);
    }

    public override void OnSubmit()
    {
        GameManager.Score = -1;
        SceneManager.LoadScene("GameSetup");
    }

}
