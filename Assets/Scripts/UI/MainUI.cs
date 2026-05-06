using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainUI : Singleton<MainUI>
    {
        [Header("Lives UI")]
        [SerializeField] private Image haveLifeSprite;
        [SerializeField] private Image noLifeSprite;
        [SerializeField] private Transform livesContainer;

        private Player.PlayerStats player;

        protected override void Awake()
        {
            base.Awake();
            
            player = FindFirstObjectByType<Player.PlayerStats>();
            if (!player) Debug.LogError("PlayerStats component not found in the scene. Please ensure there is a GameObject with PlayerStats attached.");

            UpdateLivesUI(player.CurrentLives, player.MaxLives);
        }
        
        private void OnEnable() => Player.PlayerStats.OnLivesChanged += UpdateLivesUI;
        private void OnDisable() => Player.PlayerStats.OnLivesChanged -= UpdateLivesUI;

        public void UpdateLivesUI(int currentLives, int maxLives)
        {
            foreach (Transform child in livesContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < maxLives; i++)
            {
                Image lifeIcon = Instantiate(i < currentLives ? haveLifeSprite : noLifeSprite, livesContainer);
                lifeIcon.transform.localScale = Vector3.one;
            }
        }
    }
}
