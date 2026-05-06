using System;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(VFXs))]
    public class PlayerHealthController : MonoBehaviour
    {
        public static event Action<float, float> OnHealthChanged;

        [Header("Player Health Refs")] 
        [SerializeField] private float testDamageAmount = 50f;

        private PlayerStats playerStats;
        private VFXs visualEffects;

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
            visualEffects = GetComponent<VFXs>();
        }

        private void Start()
        {
            print("Player initialized: " + playerStats.CurrentHealth + " health, " + playerStats.MaxHealth + " max health.");
            OnHealthChanged?.Invoke(playerStats.CurrentHealth, playerStats.MaxHealth);
        }

        private void Update()
        {
            TestingHealthStuff();
        }

        public void Damage(Transform source, float amount)
        {
            playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth - amount, 0, playerStats.MaxHealth);
            if (playerStats.CurrentHealth <= 0) PlayerDies();
            
            visualEffects.FlashVFX();
            visualEffects.Knockback(source, transform, rb: GetComponent<Rigidbody2D>());
            visualEffects.Fade();
        
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

        private void TestingHealthStuff()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame) Damage(transform, testDamageAmount);
            if (Keyboard.current.yKey.wasPressedThisFrame) Heal(10f);
        }
    }
}