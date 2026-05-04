using System;
using System.Collections;
using Managers;
using Misc;
using Player;
using UI;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Player Stats")]
    [SerializeField] private int gemsCollected = 0;

    private PlayerStats player;
    private CheckpointManager respwanManager;
    private FadeUI fadeUI;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
        if (!player) Debug.LogError("Player not found by Game Manager.");
        respwanManager = CheckpointManager.Instance;
        if (!respwanManager) Debug.LogError("Checkpoint Manager not found by Game Manager.");
        fadeUI = FadeUI.Instance;
        if (!fadeUI) Debug.LogError("Fade UI not found by Game Manager.");
    }

    public void AddGemCollected() => gemsCollected++;

    public void OnPlayerDeath() => StartCoroutine(PlayerRespawnSequence());
    
    private IEnumerator PlayerRespawnSequence()
    {
        // Handles player death and calls few scripts to do what it has to do. 
        // Sensetive to initialization order bugs cuz of how many scripts it has to call. 
        // Careful when changing anything related to player death and respawn.

        Time.timeScale = 0f;
        fadeUI.FadeToBlack();
        yield return new WaitForSecondsRealtime(1f);
        player.ResetPlayerStats();
        player.transform.position = respwanManager.GetCurrentRespawnPoint();
        fadeUI.FadeToClear();
        Time.timeScale = 1f;
    }
}
