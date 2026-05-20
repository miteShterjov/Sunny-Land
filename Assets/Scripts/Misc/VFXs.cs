using System.Collections;
using Enemies;
using Player;
using UnityEngine;

namespace Misc
{
    public class VFXs : MonoBehaviour
    {
        [Header("Flash VFX Settings")]
        [SerializeField] public float flashDuration = 0.3f;
        [SerializeField] private Material flashMaterial;
        [SerializeField] private Material defaultMaterial;
        [Header("Fade VFX Settings")]
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private float fadeLevel = 0.4f;
        [Header("Knockback Settings")]
        [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 7f);
        [SerializeField] private float knockbackCooldown = 0.5f;
        [Header("Flicker VFX Settings")]
        [SerializeField] private float flickerDuration = 1.5f;
        [SerializeField] private float flickerInterval = 0.1f;
        [Header("Squash & Stretch Settings")]
        [SerializeField] private float squashDuration = 0.1f;
        [SerializeField] private Vector3 squashScale = new Vector3(1.3f, 0.7f, 1f);
        [Header("Screen Shake Settings")]
        [SerializeField] private float shakeDuration = 0.2f;
        [SerializeField] private float shakeMagnitude = 0.1f;

        private SpriteRenderer spriteRenderer;
        private const float FadeFullAlpha = 1f;
        private Camera mainCamera;

        private Vector2 knockbackDirection;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        public void FlashVFX() => StartCoroutine(StartFlashVFXCoroutine());
    
        public void Fade() => StartCoroutine(FadeCoroutine());

        public void Flicker() => StartCoroutine(FlickerCoroutine());

        public void SquashAndStretch() => StartCoroutine(SquashAndStretchCoroutine());

        public void ScreenShake() => StartCoroutine(ScreenShakeCoroutine());

        public void Knockback(Transform source, Transform target, Rigidbody2D rb)
        {
            var movement = target.GetComponent<PlayerMovementController>();
            if (movement) movement.IsKnockedBack = true;
            
            rb.linearVelocity = Vector2.zero;
            float directionX = source.position.x < target.position.x ? 1f : -1f;
            rb.AddForce(new Vector2(directionX * knockbackForce.x, knockbackForce.y), ForceMode2D.Impulse);
            
            StartCoroutine(ClearKnockbackCo(movement.gameObject));
        }

        public void EnemyKnockback(Transform source, Transform target, Rigidbody2D rb)
        {
            rb.linearVelocity = Vector2.zero;
            float directionX = source.position.x < target.position.x ? 1f : -1f;
            rb.AddForce(new Vector2(directionX * knockbackForce.x, knockbackForce.y), ForceMode2D.Impulse);

            StartCoroutine(KnockbackDurationCo());
        }

        private IEnumerator ClearKnockbackCo(GameObject movement)
        {
            yield return new WaitForSeconds(knockbackCooldown);
            if (movement.gameObject.CompareTag("Player")) 
            {
                movement.gameObject.GetComponent<PlayerMovementController>().IsKnockedBack = false;
            }
            else if (movement.gameObject.CompareTag("Enemy"))
            {
                movement.gameObject.GetComponent<Enemy>().IsKnockback = false;
            }
        }

        private IEnumerator KnockbackDurationCo()
        {
            yield return new WaitForSeconds(knockbackCooldown);
        }

        private IEnumerator StartFlashVFXCoroutine()
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.material = defaultMaterial;
        }

        private IEnumerator FadeCoroutine()
        {
            Color c = spriteRenderer.color;

            spriteRenderer.color = new Color(c.r, c.g, c.b, fadeLevel);
            yield return new WaitForSeconds(fadeDuration);
            spriteRenderer.color = new Color(c.r, c.g, c.b, FadeFullAlpha);
        }

        private IEnumerator FlickerCoroutine()
        {
            float elapsed = 0f;
            while (elapsed < flickerDuration)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(flickerInterval);
                elapsed += flickerInterval;
            }
            spriteRenderer.enabled = true;
        }

        private IEnumerator SquashAndStretchCoroutine()
        {
            Vector3 originalScale = transform.localScale;
            transform.localScale = squashScale;
            yield return new WaitForSeconds(squashDuration);
            transform.localScale = originalScale;
        }

        private IEnumerator ScreenShakeCoroutine()
        {
            if (!mainCamera) yield break;
            
            Vector3 originalPos = mainCamera.transform.position;
            float elapsed = 0f;
            while (elapsed < shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;
                mainCamera.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            mainCamera.transform.position = originalPos;
        }
    }
}
