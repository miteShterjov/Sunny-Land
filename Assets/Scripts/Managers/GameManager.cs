using System.Collections;
using Misc;
using Player;
using UI;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Player Stats")]
        [SerializeField] private int gemsCollected;
        [Header("Game Prefs")]
        [SerializeField] private bool isGameOver;
        
        public bool IsGameOver => isGameOver;

        private PlayerData player;
        private CheckpointManager respawnManager;
        private FadeUI fadeUI;

        private void Start()
        {
            player = FindFirstObjectByType<PlayerData>();
            if (!player) Debug.LogError("Player not found by Game Manager.");
            respawnManager = CheckpointManager.Instance;
            if (!respawnManager) Debug.LogError("Checkpoint Manager not found by Game Manager.");
            fadeUI = FadeUI.Instance;
            if (!fadeUI) Debug.LogError("Fade UI not found by Game Manager.");
        }

        private void OnEnable() => PlayerData.OnLivesChanged += IsTheGameOver;
        private void OnDisable() => PlayerData.OnLivesChanged -= IsTheGameOver;

        public int GemsCollected { set => gemsCollected = value; get => gemsCollected; }

        public void OnPlayerDeath() => StartCoroutine(PlayerRespawnSequence());
    
        private IEnumerator PlayerRespawnSequence()
        {
            // Handles player death and calls few scripts to do what it has to do. 
            // Sensitive to initialization order bugs because of how many scripts it has to call. 
            // Careful when changing anything related to player death and respawn.
            Time.timeScale = 0f;
            fadeUI.FadeToBlack();
            yield return new WaitForSecondsRealtime(1f);
            player.ResetPlayerStats();
            player.transform.position = respawnManager.GetCurrentRespawnPoint();
            fadeUI.FadeToClear();
            Time.timeScale = 1f;
        }
        
        private void IsTheGameOver(int currentLives, int maxLives)
        {
            if (currentLives == 0) isGameOver = true;
        }
    }
}
