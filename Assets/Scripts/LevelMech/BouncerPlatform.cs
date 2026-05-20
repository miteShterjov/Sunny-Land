using System.Collections;
using UnityEngine;

public class BouncerPlatform : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private float bounceDuration = 0.5f;
    [SerializeField] private Sprite bouncedSprite;

    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;
    private bool canBounce = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        originalSprite = spriteRenderer.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (!canBounce) return;

        Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();

        if (!playerRb) return;

        Vector2 bounceDirection = Vector2.up; // Bounce upwards
        playerRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
        StartCoroutine(BounceEffect());
    }
    
    private IEnumerator BounceEffect()
    {
        canBounce = false;
        spriteRenderer.sprite = bouncedSprite;
        yield return new WaitForSeconds(bounceDuration);
        spriteRenderer.sprite = originalSprite;
        canBounce = true;
    }
}
