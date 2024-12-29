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

    private Color color;
    private int index;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        // Keeps track on its own how many buttons are there and assigns color relatively
        index = buttonCount++;
        color = ViewManager.Instance.buttonColors[index];

        // Set color
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;

        // Set audioclip
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GetComponentInParent<ViewManager>().sounds[index];
    }

    private void OnDestroy()
    {
        // Keep track how many buttons are there
        buttonCount--;
    }

    public void OnMouseDown()
    {
        if ( enabled )
        {
            StartCoroutine(ActivateButton());
            GameManager.Instance.CheckSequence(index); // Check if its the right button to press
        }
    }

    /// <summary>
    /// Plays the button's
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

