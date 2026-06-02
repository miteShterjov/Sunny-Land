using System;
using UnityEngine;

namespace LevelMech
{
    public class FlagPole : MonoBehaviour
    {
        public static event Action OnLevelFinished;
        
        [Header("Flag Settings")]
        [SerializeField] private GameObject flag;
        
        private SpriteRenderer flagSpriteRenderer;
        
        private void Awake()
        {
            flagSpriteRenderer = flag.GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            VisualEffectFlag();
            OnLevelFinished?.Invoke();
        }

        private void VisualEffectFlag() => flagSpriteRenderer.color = Color.green;
    }
}

