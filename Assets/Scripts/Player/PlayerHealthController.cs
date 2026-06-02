using System;
using System.Collections;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{   
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(VFXs))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerHealthController : MonoBehaviour
    {
        public static event Action<float, float> OnHealthChanged;
        public static event Action <bool> OnInvincibilityChanged;

        public bool IsInvincible => isInvincible;

        [Header("Player Health Refs")] 
        [SerializeField] private float testDamageAmount = 50f;
        [SerializeField] private bool isInvincible;
        [SerializeField] private float invincibilityDuration = 1f;

        private PlayerData playerStats;
        private VFXs visualEffects;
        private Rigidbody2D rb;

        private void Awake()
        {
            playerStats = GetComponent<PlayerData>();
            visualEffects = GetComponent<VFXs>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            OnHealthChanged?.Invoke(playerStats.CurrentHealth, playerStats.MaxHealth);
        }

        private void Update()
        {
            TestingHealthStuff();
        }

        public void Damage(Transform source, float amount)
        {
            if (isInvincible) return;
            
            playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth - amount, 0, playerStats.MaxHealth);
            if (playerStats.CurrentHealth <= 0) PlayerDies();
            
            visualEffects.FlashVFX();
            visualEffects.Knockback(source, transform, rb);
            visualEffects.Fade();

            StartCoroutine(InvincibilityCoroutine());
        
            OnHealthChanged?.Invoke(playerStats.CurrentHealth, playerStats.MaxHealth);
        }

        public void Heal(float amount)
        {
            if (playerStats.CurrentHealth >= playerStats.MaxHealth) 
            {   
                playerStats.CurrentHealth = playerStats.MaxHealth;
                return;
            }
            playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth + amount, 0, playerStats.MaxHealth);
            Debug.Log($"Player healed {amount}. Current health: {playerStats.CurrentHealth}/{playerStats.MaxHealth}");
            
            OnHealthChanged?.Invoke(playerStats.CurrentHealth, playerStats.MaxHealth);
        }

        private void PlayerDies() => GameManager.Instance.OnPlayerDeath();

        private IEnumerator InvincibilityCoroutine()
        {
            isInvincible = true;
            OnInvincibilityChanged?.Invoke(isInvincible);
            yield return new WaitForSeconds(invincibilityDuration);
            isInvincible = false;
            OnInvincibilityChanged?.Invoke(isInvincible);
        }

        private void TestingHealthStuff()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame) Damage(transform, testDamageAmount);
            if (Keyboard.current.yKey.wasPressedThisFrame) Heal(10f);
        }
    }
}