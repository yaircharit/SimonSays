using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GameButton : MonoBehaviour
{
    private static Color[] buttonColors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };
    private static int buttonCount = 0;

    private Color color;
    private int index;

    public void Start()
    {
        index = buttonCount++;
        color = buttonColors[index];
        
        GetComponent<SpriteRenderer>().color = color;
    }
}

