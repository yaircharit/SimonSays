using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class OverlayWindow : MonoBehaviour
    {

        public virtual void OpenWindow()
        {
            gameObject.SetActive(true);
        }
        public virtual void CloseWindow()
        {
            gameObject.SetActive(false);
        }
        public abstract void OnSubmit();
    }
}
