using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderUI : MonoBehaviour
    {
        [Header("Health Slider")]
        [SerializeField] private Image middleFill;
        [SerializeField] private Image healthFill;
        [Header("Stamina Slider")]
        [SerializeField] private Image middleStaminaFill;
        [SerializeField] private Image staminaFill;
        [Header("Mana Slider")]
        [SerializeField] private Image middleManaFill;
        [SerializeField] private Image ManaFill;
        [Header("Speed Settings")]
        [SerializeField] private float frontSpeedFill = 5f;
        [SerializeField] private float middleSpeedFill = 2.5f;
        
        private float targetHealthFill;
        private float targetStaminaFill;
        private float targetManaFill;
        private void OnEnable()
        {
            PlayerHealthController.OnHealthChanged += UpdateHealthSlider;
            PlayerData.OnStaminaChanged += UpdateStaminaSlider;
            PlayerData.OnManaChanged += UpdateManaSlider;
        }

        private void OnDisable()
        {
            PlayerHealthController.OnHealthChanged -= UpdateHealthSlider;
            PlayerData.OnStaminaChanged -= UpdateStaminaSlider;
            PlayerData.OnManaChanged -= UpdateManaSlider;
        }

        private void Update()
        {
            UpdateHealthBar();
            UpdateStaminaBar();
            UpdateManaBar();
        }

        private void UpdateManaBar()
        {
            if (targetManaFill < ManaFill.fillAmount)
            {
                ManaFill.fillAmount = Mathf.MoveTowards(ManaFill.fillAmount, targetManaFill, frontSpeedFill * Time.deltaTime);
                middleManaFill.fillAmount = Mathf.MoveTowards(middleManaFill.fillAmount, targetManaFill, middleSpeedFill * Time.deltaTime);
            }
            else
            {
                ManaFill.fillAmount = Mathf.MoveTowards(ManaFill.fillAmount, targetManaFill, middleSpeedFill * Time.deltaTime);
                middleManaFill.fillAmount = Mathf.MoveTowards(middleManaFill.fillAmount, targetManaFill, frontSpeedFill * Time.deltaTime);
            }    
        }

        private void UpdateStaminaBar()
        {
            if (targetStaminaFill < staminaFill.fillAmount)
            {
                staminaFill.fillAmount = Mathf.MoveTowards(staminaFill.fillAmount, targetStaminaFill, frontSpeedFill * Time.deltaTime);
                middleStaminaFill.fillAmount = Mathf.MoveTowards(middleStaminaFill.fillAmount, targetStaminaFill, middleSpeedFill * Time.deltaTime);
            }
            else
            {
                staminaFill.fillAmount = Mathf.MoveTowards(staminaFill.fillAmount, targetStaminaFill, middleSpeedFill * Time.deltaTime);
                middleStaminaFill.fillAmount = Mathf.MoveTowards(middleStaminaFill.fillAmount, targetStaminaFill, frontSpeedFill * Time.deltaTime);
            }    
        }

        private void UpdateHealthBar()
        {
            if (targetHealthFill < healthFill.fillAmount)
            {
                healthFill.fillAmount = Mathf.MoveTowards(healthFill.fillAmount, targetHealthFill, frontSpeedFill * Time.deltaTime);
                middleFill.fillAmount = Mathf.MoveTowards(middleFill.fillAmount, targetHealthFill, middleSpeedFill * Time.deltaTime);
            }
            else
            {
                healthFill.fillAmount = Mathf.MoveTowards(healthFill.fillAmount, targetHealthFill, middleSpeedFill * Time.deltaTime);
                middleFill.fillAmount = Mathf.MoveTowards(middleFill.fillAmount, targetHealthFill, frontSpeedFill * Time.deltaTime);
            }
        }

        private void UpdateHealthSlider(float current, float max) => targetHealthFill = max > 0 ? current / max : 0f;
        
        private void UpdateStaminaSlider(float current, float max) => targetStaminaFill = max > 0 ? current / max : 0f;

        private void UpdateManaSlider(float current, float max) => targetManaFill = max > 0 ? current / max : 0f;
    }
}