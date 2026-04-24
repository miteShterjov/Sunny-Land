using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDepletionRate = 2f; 
    [SerializeField] private float staminaRecoveryRate = 5f;
    [SerializeField] private float staminaThreshold = 10f; // Minimum stamina to start sprinting
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentStamina;
    [SerializeField] private bool spendStamina;
    private bool isStaminaExhausted = false; // Player can't sprint until stamina >= threshold 

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    private void FixedUpdate()
    {
        // Handle stamina depletion while sprinting
        if (spendStamina && !isStaminaExhausted)
        {
            currentStamina -= staminaDepletionRate * Time.fixedDeltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            
            // Check if stamina is fully depleted
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isStaminaExhausted = true;
                spendStamina = false; // Force stop sprinting
            }
        }
        // Handle stamina recovery when not sprinting
        else if (!spendStamina && currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.fixedDeltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            
            // Check if stamina has recovered enough to allow sprinting again
            if (currentStamina >= staminaThreshold && isStaminaExhausted)
            {
                isStaminaExhausted = false;
            }
        }
    }

    public float Health {
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }

    public float Stamina {
        get => currentStamina;
        set => currentStamina = Mathf.Clamp(value, 0, maxStamina);
    }

    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;

    public void SpendStamina(bool spend)
    {
        // Only allow sprinting if stamina is not exhausted
        if (spend && isStaminaExhausted)
        {
            spendStamina = false;
            return;
        }
        
        spendStamina = spend;
    }

    public bool CanSprint()
    {
        return !isStaminaExhausted && currentStamina > 0;
    }

    public float CurrentStamina => currentStamina;
    public bool IsStaminaExhausted => isStaminaExhausted;
}
