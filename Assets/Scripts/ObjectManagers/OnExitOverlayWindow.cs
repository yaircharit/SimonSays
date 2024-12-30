using Assets.Scripts;
using UnityEngine.SceneManagement;

public class OnExitOverlayWindow : OverlayWindow
{
    public override void CloseWindow()
    {
        base.CloseWindow();
        ViewManager.EnableButtons(true);
    }

    public override void OpenWindow()
    {
        base.OpenWindow();
        ViewManager.EnableButtons(false);
    }

    public override void OnSubmit()
    {
        SceneManager.LoadScene("GameSetup");
    }

}
