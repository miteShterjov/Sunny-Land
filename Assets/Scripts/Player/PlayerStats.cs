using System;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        public static event Action<float, float> OnStaminaChanged; 
        
        [Header("Player Stats")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaDepletionRate = 2f; 
        [SerializeField] private float staminaRecoveryRate = 5f;
        [SerializeField] private float staminaThreshold = 10f;
        [SerializeField] private float currentHealth;
        [SerializeField] private float currentStamina;
        [SerializeField] private bool spendStamina;
        [SerializeField] private int maxLives = 3;
        [SerializeField] private int currentLives = 3;

        private void Awake() 
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;    
        }

        private void Start()
        {
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        private void FixedUpdate()
        {
            switch (spendStamina)
            {
                case true when !IsStaminaExhausted:
                {
                    currentStamina -= staminaDepletionRate * Time.fixedDeltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            
                    if (currentStamina <= 0)
                    {
                        currentStamina = 0;
                        IsStaminaExhausted = true;
                        spendStamina = false; // Force stop sprinting
                    }

                    break;
                }
                
                case false when currentStamina < maxStamina:
                {
                    currentStamina += staminaRecoveryRate * Time.fixedDeltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            
                    if (currentStamina >= staminaThreshold && IsStaminaExhausted) IsStaminaExhausted = false;
                    
                    break;
                }
            }

            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        public float Health {
            get => currentHealth;
            set => currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }

        public float Stamina {
            get => currentStamina;
            set => currentStamina = Mathf.Clamp(value, 0, maxStamina);
        }
        
        public int CurrentLives 
        {
            get => currentLives;
            set => currentLives = Mathf.Clamp(value, 0, maxLives);
        }

        public float MaxHealth => maxHealth;
        
        public float MaxStamina => maxStamina;

        public int MaxLives => maxLives;

        public void SpendStamina(bool spend)
        {
            if (spend && IsStaminaExhausted)
            {
                spendStamina = false;
                return;
            }
        
            spendStamina = spend;
        }

        public void SpendStamina(float amount)
        {
            if (amount < 0) return; // Prevent negative stamina spending
        
            currentStamina -= amount;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);

            if (!(currentStamina <= 0)) return;
            currentStamina = 0;
            IsStaminaExhausted = true;
            spendStamina = false;
        }

        public bool CanSprint() => !IsStaminaExhausted && currentStamina > 0;
        
        public float CurrentStamina => currentStamina;
        
        private bool IsStaminaExhausted { get; set; }
    }
}
