using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class GameButton : MonoBehaviour
{
    private static int buttonCount = 0;

    public Sprite buttonSprite;
    public Sprite pressedButtonSprite;

    private int index;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    public static event Action<int> OnButtonPress;

    private void Awake()
    {
        // Keeps track on its own how many buttons are there and assigns color relatively
        index = buttonCount++;

        // Set color
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = ViewManager.ButtonColors[index];

        // Set audioclip
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ViewManager.Sounds[index];

        enabled = false; //disable until the game starts
    }

    private void OnDestroy()
    {
        // Keep track how many buttons are there
        buttonCount--;
    }


    private void OnMouseDown()
    {
        if ( enabled )
        {
            StartCoroutine(ActivateButton());
            OnButtonPress?.Invoke(index);
        }
    }

    /// <summary>
    /// Plays the button's animation
    /// </summary>
    /// <returns>Finishes after the audioclip is done playing</returns>
    public IEnumerator ActivateButton()
    {
        spriteRenderer.sprite = pressedButtonSprite;

        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);

        spriteRenderer.sprite = buttonSprite;
    }
}

