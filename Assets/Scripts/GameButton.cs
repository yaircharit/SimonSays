using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class GameButton : MonoBehaviour
{
    private static Color[] buttonColors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };
    private static int buttonCount = 0;

    public float colorCoefficient = 0.75f;
    public Sprite buttonSprite;
    public Sprite pressedButtonSprite;

    private Color color;
    private int index;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        index = buttonCount++;
        color = buttonColors[index] * colorCoefficient;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GetComponentInParent<ViewManager>().sounds[index];
    }

    private void OnDestroy()
    {
        buttonCount--;
    }

    public void OnMouseDown()
    {
        StartCoroutine(ActivateButton());
        GameManager.Instance.CheckSequence(index);
    }

    public IEnumerator ActivateButton()
    {
        spriteRenderer.color = color * 2;
        spriteRenderer.sprite = pressedButtonSprite;
        audioSource.Play();

        yield return new WaitWhile(() => audioSource.isPlaying);

        spriteRenderer.color = color;
        spriteRenderer.sprite = buttonSprite;
    }
}

