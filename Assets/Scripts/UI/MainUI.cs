using Managers;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class MainUI : Singleton<MainUI>
    {
        [Header("PlayerStats UI")]
        [SerializeField] private Image haveLifeSprite;
        [SerializeField] private Image noLifeSprite;
        [SerializeField] private Transform livesContainer;
        [SerializeField] private TMPro.TextMeshProUGUI gemsCollectedText;

        [Header("PauseMenu UI")]
        [SerializeField] private GameObject pauseMenu;

        private Player.PlayerData player;
        private PauseUI pauseUI;
        private FadeUI fadeUI;

        protected override void Awake()
        {
            base.Awake();

            player = FindFirstObjectByType<Player.PlayerData>();
            if (!player) Debug.LogError("PlayerStats component not found in MainUI.");
            else UpdateLivesUI(player.CurrentLives, player.MaxLives);

            pauseUI = pauseMenu.GetComponent<PauseUI>();
            fadeUI = GetComponentInChildren<FadeUI>();
        }

        private void Start()
        {
            if (pauseUI.IsPaused) TogglePauseMenu();
        }

        private void Update()
        {
            gemsCollectedText.text = GameManager.Instance.GemsCollected.ToString();

            if (Keyboard.current.escapeKey.wasPressedThisFrame) TogglePauseMenu();
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

        private void TogglePauseMenu()
        {
            pauseUI.IsPaused = !pauseUI.IsPaused;
            pauseMenu.SetActive(pauseUI.IsPaused);
            fadeUI.gameObject.SetActive(!pauseUI.IsPaused);
        }
    }
}
