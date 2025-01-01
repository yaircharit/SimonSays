using UnityEngine;

namespace Assets.Scripts
{
    public abstract class OverlayWindow : MonoBehaviour
    {
        internal bool IsActive => gameObject.activeSelf;

        public void OpenWindow()
        {
            gameObject.SetActive(true);
            OnOpen();
        }

        public void CloseWindow()
        {
            OnClose();
            gameObject.SetActive(false);
        }

        protected virtual void OnOpen()
        {
            // Optional hook for subclasses to implement behavior when the window opens
        }

        protected virtual void OnClose()
        {
            // Optional hook for subclasses to implement behavior when the window closes
        }

        public abstract void OnSubmit(); // Should be different for each overlay window type.
    }
}
