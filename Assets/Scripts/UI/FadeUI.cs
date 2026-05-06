using System.Collections;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FadeUI : Singleton<FadeUI>
    {
        [Header("Fade Settings")]
        [SerializeField] private float fadeSpeed = 1f;
        [SerializeField] private Image fadeScreen;

        private IEnumerator fadeRoutine;

        protected override void Awake()
        {
            base.Awake();
            if (!fadeScreen) Debug.LogError("Fade screen Image component is not assigned in the inspector.");
        }

        public void FadeToBlack()
        {
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            
            fadeRoutine = FadeRoutine(1);
            StartCoroutine(fadeRoutine);
        }

        public void FadeToClear()
        {
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            
            fadeRoutine = FadeRoutine(0);
            StartCoroutine(fadeRoutine);
        }

        private IEnumerator FadeRoutine(float targetAlpha)
        {
            while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
            {
                float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.unscaledDeltaTime);
                fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
                yield return null;
            }

        }

    }
}