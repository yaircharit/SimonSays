using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI buttonText;
    [SerializeField]
    private UnityEngine.UI.Button button;

    public string ButtonText
    {
        get => buttonText.text;
        set => buttonText.text = value;
    }
    public UnityEngine.UI.Button Button => button;
}
