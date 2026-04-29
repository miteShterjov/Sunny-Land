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
        [Header("Speed Settings")]
        [SerializeField] private float frontSpeedFill = 5f;
        [SerializeField] private float middleSpeedFill = 2.5f;
        
        private float targetHealthFill;
        private float targetStaminaFill;

        private void OnEnable()
        {
            PlayerHealthController.OnHealthChanged += UpdateHealthSlider;
            PlayerStats.OnStaminaChanged += UpdateStaminaSlider;
        }

        private void OnDisable()
        {
            PlayerHealthController.OnHealthChanged -= UpdateHealthSlider;
            PlayerStats.OnStaminaChanged -= UpdateStaminaSlider;
        }

        private void Update()
        {
            UpdateHealthBar();
            UpdateStaminaBar();
        }

        private void UpdateStaminaBar()
        {
            staminaFill.fillAmount = Mathf.MoveTowards(staminaFill.fillAmount, targetStaminaFill, frontSpeedFill * Time.deltaTime);
            middleStaminaFill.fillAmount = Mathf.MoveTowards(middleStaminaFill.fillAmount, targetHealthFill, middleSpeedFill * Time.deltaTime);
        }

        private void UpdateHealthBar()
        {
            healthFill.fillAmount = Mathf.MoveTowards(healthFill.fillAmount, targetHealthFill, frontSpeedFill * Time.deltaTime);
            middleFill.fillAmount = Mathf.MoveTowards(middleFill.fillAmount, targetHealthFill, middleSpeedFill * Time.deltaTime);
        }

        private void UpdateHealthSlider(float current, float max) => targetHealthFill = current / max;
        
        private void UpdateStaminaSlider(float current, float max) => targetStaminaFill = current / max;
    }
}