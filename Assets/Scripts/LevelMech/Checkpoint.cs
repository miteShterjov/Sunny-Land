using System;
using UnityEngine;

namespace LevelMech
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Checkpoint : MonoBehaviour
    {
        public static event Action<Checkpoint> OnSpawnStatusChanged;

        [Header("Respawn Point Sprites")]
        [SerializeField] private Sprite respawnOff;
        [SerializeField] private Sprite respawnOn;

        private SpriteRenderer spriteRenderer;
        private bool isActiveRespawnPoint;
        private bool wasActiveSpawnPoint;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (wasActiveSpawnPoint) return;
            if (isActiveRespawnPoint) return;

            OnSpawnStatusChanged?.Invoke(this);
        }

        public void ActivateRespawnPoint()
        {
            isActiveRespawnPoint = true;
            spriteRenderer.sprite = respawnOn;
        }

        public void DeactivateRespawnPoint()
        {
            isActiveRespawnPoint = false;
            wasActiveSpawnPoint = true;
            spriteRenderer.sprite = respawnOff;
        }
    }
}
