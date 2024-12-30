using UnityEngine;

namespace Assets.Scripts
{
    public abstract class OverlayWindow : MonoBehaviour
    {
        internal bool IsActive => gameObject.activeSelf;

        public virtual void OpenWindow()
        {
            gameObject.SetActive(true);
        }
        public virtual void CloseWindow()
        {
            gameObject.SetActive(false);
        }
        public abstract void OnSubmit(); // Should be different for each overlay window type.
    }
}
