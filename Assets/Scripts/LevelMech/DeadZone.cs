using System;
using Managers;
using Player;
using UnityEngine;

namespace LevelMech
{
    public class DeadZone : MonoBehaviour
    {
        [Header("Destroy Settings")]
        [SerializeField] private float destroyDelay = 0f;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            GameManager.Instance.OnPlayerDeath();
            other.gameObject.SetActive(false);
        }
    }
}
