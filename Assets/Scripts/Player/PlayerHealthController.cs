using System;
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
        private float currentHealth;
        private float maxHealth;

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
        }

        private void Start()
        {
            maxHealth = playerStats.MaxHealth;
            currentHealth = playerStats.Health;
            print("Player initialized: " + currentHealth + " health, " + maxHealth + " max health.");
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        private void Update()
        {
            TestingHealthStuff();
        }

        public void Damage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0) PlayerDies();
            Debug.Log($"Player took {amount} damage. Current health: {currentHealth}/{maxHealth}");
        
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        private void Heal(float amount)
        {
            if (currentHealth >= maxHealth) 
            {   
                currentHealth = maxHealth;
                return;
            }
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            Debug.Log($"Player healed {amount}. Current health: {currentHealth}/{maxHealth}");
            
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        private void PlayerDies()
        {
            Debug.Log("Player has died.");
        }

        private void TestingHealthStuff()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame) Damage(10f);
            if (Keyboard.current.yKey.wasPressedThisFrame) Heal(10f);
        }
    }
}