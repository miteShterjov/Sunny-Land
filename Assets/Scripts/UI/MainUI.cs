using Managers;
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
        [SerializeField] private TMPro.TextMeshProUGUI gemsCollectedText;

        private Player.PlayerData player;

        protected override void Awake()
        {
            base.Awake();
            
            player = FindFirstObjectByType<Player.PlayerData>();
            if (!player) Debug.LogError("PlayerStats component not found in MainUI.");
            else UpdateLivesUI(player.CurrentLives, player.MaxLives);
        }

        private void Update()
        {
            gemsCollectedText.text = GameManager.Instance.GemsCollected.ToString();
        }

        private void OnEnable() => Player.PlayerData.OnLivesChanged += UpdateLivesUI;
        private void OnDisable() => Player.PlayerData.OnLivesChanged -= UpdateLivesUI;

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
