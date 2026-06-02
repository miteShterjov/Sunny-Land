using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseUI : MonoBehaviour
    {

        [Header("PauseMenu Config")]
        [SerializeField] private Button resumeGame;
        [SerializeField] private Button quitGame;
        [SerializeField] private Button muteGame;
        [SerializeField] private TMP_Text tipsText;

        public bool IsPaused
        {
            get => isPaused;
            set
            {
                isPaused = value;
                Time.timeScale = isPaused ? 0f : 1f;
            }
        }

        private bool isPaused;
        private string[] tips =
        {
            "Press Space to jump.",
            "Hold Shift to sprint.",
            "Stamina is shared between jumping and sprinting — spend it wisely.",
            "Explore every corner, some gems are hidden off the beaten path.",
            "Enemies have patterns. Watch before you engage."
        };


        private void OnEnable()
        {
            IsPaused = true;
            UpdateTipsText();
        }

        public void ResumeGameButton()
        {
            IsPaused = false;
            gameObject.SetActive(false);
        }

        public void QuitGameButton()
        {
            print("When I build a Main Menu scene, then I`ll come back and finish this method.");
        }

        public void MuteGameButton()
        {
            print("Same as QuitGame Button, finished when I will add Audio effects.");
        }

        private void UpdateTipsText() => tipsText.text = tips[Random.Range(0, tips.Length)];
    }
}