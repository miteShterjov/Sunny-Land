using System;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerHealthController : MonoBehaviour
    {
        // passes currentHealth, maxHealth so SlideUI can normalize it (0–1)
        public static event Action<float, float> OnHealthChanged; 

        private PlayerStats playerStats;
        private VFXs visualEffects;

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
            visualEffects = GetComponent<VFXs>();
        }

        private void Start()
        {
            print("Player initialized: " + playerStats.Health + " health, " + playerStats.MaxHealth + " max health.");
            OnHealthChanged?.Invoke(playerStats.Health, playerStats.MaxHealth);
        }

        private void Update()
        {
            TestingHealthStuff();
        }

        public void Damage(Transform source, float amount)
        {
            playerStats.Health = Mathf.Clamp(playerStats.Health - amount, 0, playerStats.MaxHealth);
            if (playerStats.Health <= 0) PlayerDies();
            Debug.Log($"Player took {amount} damage. Current health: {playerStats.Health}/{playerStats.MaxHealth}");

            // visualEffects.FlashVFX();
            visualEffects.Knockback(source, transform, rb: GetComponent<Rigidbody2D>());
            // visualEffects.Fade();
        
            OnHealthChanged?.Invoke(playerStats.Health, playerStats.MaxHealth);
        }

        public void Heal(float amount)
        {
            if (playerStats.Health >= playerStats.MaxHealth) 
            {   
                playerStats.Health = playerStats.MaxHealth;
                return;
            }
            playerStats.Health = Mathf.Clamp(playerStats.Health + amount, 0, playerStats.MaxHealth);
            Debug.Log($"Player healed {amount}. Current health: {playerStats.Health}/{playerStats.MaxHealth}");
            
            OnHealthChanged?.Invoke(playerStats.Health, playerStats.MaxHealth);
        }

        private void PlayerDies() => GameManager.Instance.OnPlayerDeath();

        private void TestingHealthStuff()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame) Damage(transform, 10f);
            if (Keyboard.current.yKey.wasPressedThisFrame) Heal(10f);
        }
    }
}