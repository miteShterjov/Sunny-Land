using System;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelMech
{   
    public class Checkpoint : MonoBehaviour
    {
        public static event Action<Checkpoint> OnSpawnStatusChanged;

        [Header("Respawn Point Sprites")]
        [SerializeField] private Sprite respawnOff;
        [SerializeField] private Sprite respawnOn;

        private Vector3 _respawnPoint;
        private SpriteRenderer _spriteRenderer;
        private bool _isActiveRespawnPoint;
        private bool _wasActiveSpawnPoint;

        private void Awake()
        {
            _respawnPoint = transform.position;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_wasActiveSpawnPoint) return;
            if (_isActiveRespawnPoint) return;

            OnSpawnStatusChanged?.Invoke(this);
        }

        public void ActivateRespawnPoint()
        {
            _isActiveRespawnPoint = true;
            _spriteRenderer.sprite = respawnOn;
        }

        public void DeactivateRespawnPoint()
        {
            _isActiveRespawnPoint = false;
            _wasActiveSpawnPoint = true;
            _spriteRenderer.sprite = respawnOff;
        }
    }
}
