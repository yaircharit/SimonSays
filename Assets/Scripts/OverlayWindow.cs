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

        public void OpenWindow()
        {
            gameObject.SetActive(true);
        }
        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
        public abstract void OnSubmit();
    }
}
