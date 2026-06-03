using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [Header("GameOver Config")]
        [SerializeField] private TMP_Text tipsText;
        
        private readonly string[] tips =
        {
            "Press Space to jump.",
            "Hold Shift to sprint.",
            "Stamina is shared between jumping and sprinting — spend it wisely.",
            "Explore every corner, some gems are hidden off the beaten path.",
            "Enemies have patterns. Watch before you engage."
        };

        private void OnEnable() => UpdateTipsText();
        
        public void RetryButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        public void MainMenuButton() => SceneManager.LoadScene("MainMenu");
        
        public void MuteGameButton()
        {
            print("Same as QuitGame Button, finished when I will add Audio effects.");
        }
        
        private void UpdateTipsText() => tipsText.text = tips[Random.Range(0, tips.Length)];
    }
}
