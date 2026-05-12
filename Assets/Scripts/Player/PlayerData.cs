using System;
using UnityEngine;

namespace Player
{
    public class PlayerData : MonoBehaviour
    {
        public static event Action<float, float> OnStaminaChanged;
        public static event Action<int, int> OnLivesChanged;
        public static event Action<float, float> OnMagikaChanged;

        [Header("Player Stats")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float maxMagika = 100f;
        [SerializeField] private float staminaDepletionRate = 2f;
        [SerializeField] private float staminaRecoveryRate = 5f;
        [SerializeField] private float staminaThreshold = 10f;
        [SerializeField] private float magikaRecoveryRate = 2.5f;
        [SerializeField] private float currentHealth;
        [SerializeField] private float currentStamina;
        [SerializeField] private float currentMagika;
        [SerializeField] private bool spendStamina;
        [SerializeField] private int maxLives = 3;
        [SerializeField] private int currentLives = 3;

        private void Awake()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentMagika = maxMagika;
        }

        private void Start()
        {
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            OnLivesChanged?.Invoke(currentLives, maxLives);
            OnMagikaChanged?.Invoke(currentMagika, maxMagika);
        }

        private void FixedUpdate()
        {
            HandleStamina();
            HandleMagika();
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        public float CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }

        public float CurrentStamina
        {
            get => currentStamina;
            set => currentStamina = Mathf.Clamp(value, 0, maxStamina);
        }

        public float CurrentMagika
        {
            get => currentMagika;
            set
            {
                currentMagika = Mathf.Clamp(value, 0, maxMagika);
                OnMagikaChanged?.Invoke(currentMagika, maxMagika);
            }
        }

        public int CurrentLives
        {
            get => currentLives;
            set => currentLives = Mathf.Clamp(value, 0, maxLives);
        }

        public float MaxHealth => maxHealth;

        public float MaxStamina => maxStamina;
        public float MaxMagika => maxMagika;
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

        public void ResetPlayerStats()
        {
            GetComponent<PlayerHealthController>().Heal(maxHealth);
            currentStamina = maxStamina;
            RemoveOneLife();
            IsStaminaExhausted = false;
            spendStamina = false;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        public void AddOneLife()
        {
            if (currentLives >= maxLives) return;
            currentLives++;
            OnLivesChanged?.Invoke(currentLives, maxLives);
        }

        private void RemoveOneLife()
        {
            if (currentLives <= 0) return;
            currentLives--;
            OnLivesChanged?.Invoke(currentLives, maxLives);
        }

        private bool IsStaminaExhausted { get; set; }

        private void HandleStamina()
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
        }

        private void HandleMagika()
        {
            if (CurrentMagika < maxMagika)
            {
                CurrentMagika += magikaRecoveryRate * Time.fixedDeltaTime;
                CurrentMagika = Mathf.Clamp(CurrentMagika, 0, maxMagika);
            }
        }
    }
}
