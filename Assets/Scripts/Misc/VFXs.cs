using System.Collections;
using UnityEngine;

namespace Misc
{
    public class VFXs : MonoBehaviour
    {
        [Header("Flash VFX Settings")]
        [SerializeField] private float flashDuration = 0.3f;
        [SerializeField] private Material flashMaterial;
        [SerializeField] private Material defaultMaterial;
        [Header("Fade VFX Settings")]
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private float fadeLevel = 0.4f;
        [Header("Knockback Settings")]
        [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 7f);
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

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void FlashVFX() => StartCoroutine(StartFlashVFXCoroutine());
    
        public void Fade() => StartCoroutine(FadeCoroutine());

        public void Flicker() => StartCoroutine(FlickerCoroutine());

        public void SquashAndStretch() => StartCoroutine(SquashAndStretchCoroutine());

        public void ScreenShake() => StartCoroutine(ScreenShakeCoroutine());

        public void Knockback(Transform source, Transform target, Rigidbody2D rb)
        {
            Vector2 direction = (target.position - source.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }

        private IEnumerator StartFlashVFXCoroutine()
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.material = defaultMaterial;
        }

        private IEnumerator FadeCoroutine()
        {
            Color fadeColor = new Color (0, 0, 0, fadeLevel);

            spriteRenderer.color = fadeColor;
            yield return new WaitForSeconds(fadeDuration);
            spriteRenderer.color = new Color(0, 0, 0, FadeFullAlpha);
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
