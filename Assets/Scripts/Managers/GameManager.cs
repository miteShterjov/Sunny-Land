using System.Collections;
using LevelMech;
using Player;
using ScriptableObjects;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameManager : Misc.Singleton<GameManager>
    {
        public LevelCollection LevelsCollection { get => levelsCollection; set => levelsCollection = value;}
        
        private static readonly WaitForSecondsRealtime WaitFor1SecRealtime = new WaitForSecondsRealtime(1f);
        
        [Header("Player Stats")]
        [SerializeField] private int gemsCollected;
        [Header("Game Prefs")]
        [SerializeField] private bool isGameOver;
        [Header("Levels Collection")] 
        [SerializeField] private LevelCollection levelsCollection;
        
        public bool IsGameOver => isGameOver;

        private PlayerData player;
        private CheckpointManager respawnManager;
        private FadeUI fadeUI;

        private void Start()
        {
            levelsCollection = levelsCollection = Resources.Load<LevelCollection>("LevelCollection");
            
            if (SceneManager.GetActiveScene().name == "Overworld")
            {
                respawnManager = null;
                player = null;
            }
            else
            {
                player = FindFirstObjectByType<PlayerData>();
                if (!player) Debug.LogError("Player not found by Game Manager.");
                respawnManager = FindFirstObjectByType<CheckpointManager>();
                if (!respawnManager) Debug.LogError("Checkpoint Manager not found by Game Manager.");
            }
            
            fadeUI = FadeUI.Instance;
            if (!fadeUI) Debug.LogError("Fade UI not found by Game Manager.");

            levelsCollection.levels[0].isUnlocked = true;
        }

        private void Update()
        {
            if (isGameOver) OnGameOver();
        }

        private void OnEnable()
        {
            PlayerData.OnLivesChanged += IsTheGameOver;
            FlagPole.OnLevelFinished += OnLevelFinished;
        }

        private void OnDisable()
        {
            PlayerData.OnLivesChanged -= IsTheGameOver;
            FlagPole.OnLevelFinished -= OnLevelFinished;
        } 

        public int GemsCollected { set => gemsCollected = value; get => gemsCollected; }

        public void OnPlayerDeath() => StartCoroutine(PlayerRespawnSequence());

        public void OnLevelFinished() => StartCoroutine(DoLevelSwichCo());
        
        private void OnGameOver()
        {
            // to do: create a UI with options to restart the game or go to main menu
            Debug.Log("Game Over");
            isGameOver = false;
        }

        private IEnumerator DoLevelSwichCo()
        {
            //pause game->fade out->load overworld->fade in->resume game
            //call method in gameprogressmanager to load next level
            //in same method we need to pass info about current level
            Time.timeScale = 0f;
            fadeUI.FadeToBlack();
            yield return WaitFor1SecRealtime;
            fadeUI.FadeToClear();
            Time.timeScale = 1f;
            SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
        }
    
        private IEnumerator PlayerRespawnSequence()
        {
            // Handles player death and calls few scripts to do what it has to do. 
            // Sensitive to initialization order bugs because of how many scripts it has to call. 
            // Careful when changing anything related to player death and respawn.
            Time.timeScale = 0f;
            fadeUI.FadeToBlack();
            yield return WaitFor1SecRealtime;
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
