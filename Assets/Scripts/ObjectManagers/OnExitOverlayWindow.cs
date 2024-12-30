using Assets.Scripts;
using UnityEngine.SceneManagement;

public class OnExitOverlayWindow : OverlayWindow
{
    public override void OnSubmit()
    {
        SceneManager.LoadScene("GameSetup");
    }

}
